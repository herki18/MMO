using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using MMO.Base.Infrastructure;
using NUnit.Framework;
using FluentAssertions;

namespace MMO.Tests.Base {
    [TestFixture]
    public class ComponentMapTests {
        class TestAutoMapAssemblyAttribute : Attribute { }

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

        [TestAutoMapAssembly]
        interface ITestComponent4 {
            void VoidMethodNoParams();
            void VoidMethodOneParam(string param1);
            void VoidMethodTwoParams(bool param1, int param2);
        }

        [TestAutoMapAssembly]
        interface ITestComponent5
        {
        }

        [TestAutoMapAssembly]
        interface ITestComponent6
        {
        }

        interface ITestComponent7 {
            void NoResponse();
            IRpcResponse ResponseNoResult();
            IRpcResponse<int> ResponseWithResult();
        }

        class TestRpcResponse : IRpcResponse {
             
        }

        class TestRpcResponse<T> : IRpcResponse<T>
        { }

        class TestComponent7 : ITestComponent7 {
            public IRpcResponse Response1 { get; set; }
            public IRpcResponse<int> Response2 { get; set; }

            public void NoResponse() { }

            public IRpcResponse ResponseNoResult() {
                return Response1;
            }

            public IRpcResponse<int> ResponseWithResult() {
                return Response2;
            }
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

        [Test]
        public void CanInvokeVoidMethodOnComponent() {
            var map = CreateTestComponent3ComponentMap();
            var componenObject = new TestComponent3();

            map.Methods[0][0].Invoke(componenObject, null);

            componenObject.VoidMethodNoParamsCallCount.Should().Be(1);
        }

        [Test]
        public void CanInvokeVoidMethodMultipleTimesOnComponent() {
            const int targetInvokeCount = 30;

            var map = CreateTestComponent3ComponentMap();
            var componenObject = new TestComponent3();

            for (int i = 0; i < targetInvokeCount; i++) {
                map.Methods[0][0].Invoke(componenObject, null);
            }
            

            componenObject.VoidMethodNoParamsCallCount.Should().Be(targetInvokeCount);
        }

        [Test]
        public void CanInvokeVoidMethodOneParamOnComponent() {
            const string param1 = "Hello World!";

            var map = CreateTestComponent3ComponentMap();
            var componentObject = new TestComponent3();

            map.Methods[0][1].Invoke(componentObject, new object[] {param1});

            componentObject.VoidMethodOneParamCallCount.Should().Be(1);
            componentObject.VoidMethodOneParamParam1.Should().Be(param1);
        }

        [Test]
        public void CanInvokeVoidMethodTwoParamsOnComponent() {
            const bool param1 = true;
            const int param2 = 32789;

            var map = CreateTestComponent3ComponentMap();
            var componentObject = new TestComponent3();

            map.Methods[0][2].Invoke(componentObject, new object[] { param1, param2 });

            componentObject.VoidMethodTwoParamsCallCount.Should().Be(1);
            componentObject.VoidMethodTwoParamsParam1.Should().Be(param1);
            componentObject.VoidMethodTwoParamsParam2.Should().Be(param2);
        }

        [Test]
        public void ReturnValuesDoGetPassedBackFromMappedMethod() {
            var map = new ComponentMap();
            var component = map.MapComponent(typeof (ITestComponent7), 0);
            component.MapMethod(typeof (ITestComponent7).GetMethod("ResponseNoResult"), 0);

            var obj = new TestComponent7 {
                Response1 = new TestRpcResponse(), 
                Response2 = new TestRpcResponse<int>()
            };

            var result = map.Methods[0][0].Invoke(obj, new object[0]);

            result.Should().BeSameAs(obj.Response1);
        }

        [Test]
        public void ReturnValueWithResultDoGetPassedBackFromMappedMethod()
        {
            var map = new ComponentMap();
            var component = map.MapComponent(typeof(ITestComponent7), 0);
            component.MapMethod(typeof(ITestComponent7).GetMethod("ResponseWithResult"), 0);

            var obj = new TestComponent7 {
                Response1 = new TestRpcResponse(), 
                Response2 = new TestRpcResponse<int>()
            };

            var result = map.Methods[0][0].Invoke(obj, new object[0]);

            result.Should().BeSameAs(obj.Response2);
        }

        [Test]
        public void ReservedComponentIdLimitPreventsComponentsFromBeingMapped() {
            var map = new ComponentMap(3);

            Action action1 = () => map.MapComponent(typeof (ITestComponent), 0);
            Action action2 = () => map.MapComponent(typeof (ITestComponent), 1);
            Action action3 = () => map.MapComponent(typeof (ITestComponent), 2);

            action1.ShouldThrow<ArgumentException>();
            action2.ShouldThrow<ArgumentException>();
            action3.ShouldThrow<ArgumentException>();

            map.MapComponent(typeof (ITestComponent), 3);
        }

        [Test]
        public void CanAutoMapComponent() {
            var map = new ComponentMap();

            map.AutoMapComponent(typeof (ITestComponent4));
            map.Components[0].Type.Should().Be(typeof (ITestComponent4));
            map.Components[0].Methods[0].MethodInfo.Name.Should().Be("VoidMethodNoParams");
            map.Components[0].Methods[1].MethodInfo.Name.Should().Be("VoidMethodOneParam");
            map.Components[0].Methods[2].MethodInfo.Name.Should().Be("VoidMethodTwoParams");

        }

        [Test]
        public void AutoMappingComponentRespectsReservedIds() {
            var map = new ComponentMap(3);
            map.AutoMapComponent(typeof (ITestComponent4));

            map.Components[0].Should().Be(null);
            map.Components[1].Should().Be(null);
            map.Components[2].Should().Be(null);

            map.Components[3].Type.Should().Be(typeof(ITestComponent4));
            map.Components[3].Methods[0].MethodInfo.Name.Should().Be("VoidMethodNoParams");
            map.Components[3].Methods[1].MethodInfo.Name.Should().Be("VoidMethodOneParam");
            map.Components[3].Methods[2].MethodInfo.Name.Should().Be("VoidMethodTwoParams");

        }

        [Test]
        public void MappedMethodIdentifiesVoidReturnType() {
            var map = new ComponentMap();
            var component = map.MapComponent(typeof (ITestComponent7), 0);
            component.MapMethod(typeof (ITestComponent7).GetMethod("NoResponse"), 0);
            component.MapMethod(typeof (ITestComponent7).GetMethod("ResponseNoResult"), 1);
            component.MapMethod(typeof (ITestComponent7).GetMethod("ResponseWithResult"), 2);

            map.Methods[0][0].ReturnType.Should().Be(MappedMethodReturnType.Void);
            map.Methods[0][0].ResultType.Should().BeNull();
        }

        [Test]
        public void MappedMethodIdentifiesIRpcResponseReturnType()
        {
            var map = new ComponentMap();
            var component = map.MapComponent(typeof(ITestComponent7), 0);
            component.MapMethod(typeof(ITestComponent7).GetMethod("NoResponse"), 0);
            component.MapMethod(typeof(ITestComponent7).GetMethod("ResponseNoResult"), 1);
            component.MapMethod(typeof(ITestComponent7).GetMethod("ResponseWithResult"), 2);

            map.Methods[0][1].ReturnType.Should().Be(MappedMethodReturnType.Response);
            map.Methods[0][1].ResultType.Should().BeNull();
        }

        [Test]
        public void MappedMethodIdentifiesIRpcResponseWithResultReturnType()
        {
            var map = new ComponentMap();
            var component = map.MapComponent(typeof(ITestComponent7), 0);
            component.MapMethod(typeof(ITestComponent7).GetMethod("NoResponse"), 0);
            component.MapMethod(typeof(ITestComponent7).GetMethod("ResponseNoResult"), 1);
            component.MapMethod(typeof(ITestComponent7).GetMethod("ResponseWithResult"), 2);

            map.Methods[0][2].ReturnType.Should().Be(MappedMethodReturnType.ResponseWithResult);
            map.Methods[0][2].ResultType.Should().Be(typeof(int));
        }

        [Test]
        public void AutoMappingAssemblyAutoMapAssembly() {
            var map = new ComponentMap();

            map.AutoMapAssembly(typeof (ComponentMapTests).Assembly, typeof(TestAutoMapAssemblyAttribute));

            map.GetComponent(typeof (ITestComponent4)).Should().NotBeNull();
            map.GetComponent(typeof (ITestComponent5)).Should().NotBeNull();
            map.GetComponent(typeof (ITestComponent6)).Should().NotBeNull();

            Action fail = () => map.GetComponent(typeof (ITestComponent));
            fail.ShouldThrow<Exception>();
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