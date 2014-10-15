using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using MMO.Base.Infrastructure;
using NUnit.Framework;
using FluentAssertions;

namespace MMO.Tests.Base {
    [TestFixture]
    public class ComponentMapTests {

        interface ITestComponent {   
        }

        interface ITestComponent2 {
            void Method();
            void Method2();
        }

        interface ITestComponent3 {
            void VoidMethodNoParams();
            void VoidMethodOneParam(string param1);
            void VoidMethodTwoParams(bool param1, int param2);
        }

        public class TestComponent3 : ITestComponent3 {
            public int VoidMethodNoParamsCallCount { get; private set; }
            public int VoidMethodOneParamCallCount { get; private set; }
            public int VoidMethodTwoParamsCallCount { get; private set; }

            public string VoidMethodOneParamParam1 { get; private set; }
            public bool VoidMethodTwoParamsParam1 { get; private set; }
            public int VoidMethodTwoParamsParam2 { get; private set; }

            public void VoidMethodNoParams() {
                VoidMethodNoParamsCallCount++;
            }

            public void VoidMethodOneParam(string param1) {
                VoidMethodOneParamCallCount++;
                VoidMethodOneParamParam1 = param1;
            }

            public void VoidMethodTwoParams(bool param1, int param2) {
                VoidMethodTwoParamsCallCount++;
                VoidMethodTwoParamsParam1 = param1;
                VoidMethodTwoParamsParam2 = param2;
            }
        }


        [Test]
        [TestCase(typeof (ITestComponent), byte.MinValue)]
        [TestCase(typeof (ITestComponent), byte.MaxValue)]
        [TestCase(typeof (ITestComponent), 153)]
        [TestCase(typeof (ITestComponent), 1)]
        public void ManuallyMapsComponent(Type componentType, byte componentId) {
            var componentMap = new ComponentMap();

            componentMap.MapComponent(componentType, componentId);

            var mappedComponent = componentMap.Components[componentId];
            mappedComponent.Should().NotBeNull();
            mappedComponent.Type.Should().Be(componentType);
            mappedComponent.Id.Should().Be(componentId);
        }

        [Test]
        public void MappingTwoComponentsWithSameIdThrowException() {
            var componentMap = new ComponentMap();

            componentMap.MapComponent(typeof (ITestComponent), 12);

            Action failCondition = () => componentMap.MapComponent(typeof(ITestComponent), 12);
            failCondition.ShouldThrow<ArgumentException>();
        }

        [Test]
        [TestCase(typeof (ITestComponent), byte.MinValue)]
        [TestCase(typeof (ITestComponent), byte.MaxValue)]
        [TestCase(typeof (ITestComponent), 153)]
        [TestCase(typeof (ITestComponent), 1)]
        public void CanGetMappedComponentViaType(Type componentType, byte componentId) {
            var componentMap = new ComponentMap();

            componentMap.MapComponent(componentType, componentId);

            var mappedComponent = componentMap.GetComponent(componentType);

            mappedComponent.Should().NotBeNull();
            mappedComponent.Type.Should().Be(componentType);
            mappedComponent.Id.Should().Be(componentId);
        }

        [Test]
        public void MappingToComponentWithSameTypeThrowException() {
            var componentMap = new ComponentMap();

            componentMap.MapComponent(typeof(ITestComponent), 1);
            Action failCondition = () => componentMap.MapComponent(typeof(ITestComponent), 2);

            failCondition.ShouldThrow<Exception>();
        }

        [Test]
        [TestCase(byte.MinValue)]
        [TestCase(byte.MaxValue)]
        [TestCase(153)]
        [TestCase(1)]
        public void ManuallyMapsMethodsToComponent(byte id) {
            var componentMap = new ComponentMap();
            var component = componentMap.MapComponent(typeof (ITestComponent2), 4);

            var methodInfo = typeof (ITestComponent2).GetMethod("Method");
            component.MapMethod(methodInfo, id);

            var method = component.Methods[id];
            method.Id.Should().Be(id);
            method.Component.Should().Be(component);
            method.MethodInfo.Should().BeSameAs(methodInfo);
        }

        [Test]
        public void MappingTwoMethodsWithSameIdThrowException() {
            var componentMap = new ComponentMap();
            var component = componentMap.MapComponent(typeof (ITestComponent2), 4);

            component.MapMethod(typeof (ITestComponent2).GetMethod("Method"), 1);
            Action failCondition = () => component.MapMethod(typeof (ITestComponent2).GetMethod("Method2"), 1);
            failCondition.ShouldThrow<Exception>();
        }

        [Test]
        public void MappingMethodRequiresThatMethodExistsOnParentComponent() {
            var componentMap = new ComponentMap();
            var component = componentMap.MapComponent(typeof (ITestComponent), 4);

            Action fail = () => component.MapMethod(typeof (ITestComponent2).GetMethod("Method"), 5);
            fail.ShouldThrow<Exception>();
        }

        [Test]
        [TestCase(byte.MinValue)]
        [TestCase(byte.MaxValue)]
        [TestCase(153)]
        [TestCase(1)]
        public void CanGetMappedMethodViaMethodInfo(byte methodId) {
            var componentMap = new ComponentMap();
            var component = componentMap.MapComponent(typeof (ITestComponent2), 2);

            var methodInfo = typeof(ITestComponent2).GetMethod("Method");
            var method = component.MapMethod(methodInfo, methodId);

            component.GetMethod(methodInfo).Should().Be(method);
        }

        [Test]
        public void MappingTwoMethodsWithSameMethodInfoThrowsException() {
            var componentMap = new ComponentMap();
            var component = componentMap.MapComponent(typeof(ITestComponent2), 4);

            var methodInfo = typeof(ITestComponent2).GetMethod("Method");
            component.MapMethod(methodInfo, 4);

            Action fail = () => component.MapMethod(methodInfo, 5);
            fail.ShouldThrow<Exception>();
        }

        [Test]
        public void CanAccessMethodFromComponentMap() {
            var componentMap = new ComponentMap();
            var component = componentMap.MapComponent(typeof (ITestComponent2), 3);

            var mappedMethod = component.MapMethod(typeof (ITestComponent2).GetMethod("Method"), 5);

            componentMap.Methods[3][5].Should().BeSameAs(mappedMethod);
        }

        private ComponentMap CreateTestComponent3ComponentMap() {
            var componentMap = new ComponentMap();
            var component = componentMap.MapComponent(typeof (ITestComponent3), 0);
            component.MapMethod(typeof (ITestComponent3).GetMethod("VoidMethodNoParams"), 0);
            component.MapMethod(typeof (ITestComponent3).GetMethod("VoidMethodOneParam"), 1);
            component.MapMethod(typeof (ITestComponent3).GetMethod("VoidMethodTwoParams"), 2);

            return componentMap;
        }

    }
}