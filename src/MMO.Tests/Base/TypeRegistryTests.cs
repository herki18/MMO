using System;
using FluentAssertions;
using MMO.Base.Infrastructure;
using NUnit.Framework;

namespace MMO.Tests.Base {
    [TestFixture]
    public class TypeRegistryTests {
        public class TestSystemBase<TServerInterface, TClientInterface> : ISystemBase<TServerInterface, TClientInterface> {
            public Type ServerSystemInterfaceType { get { return typeof (TServerInterface); } }
            public Type ClientSystemInterfaceType { get { return typeof (TClientInterface); } }
        }

        public interface ITestSystem1Server {
             
        }

        public interface ITestSystem1Client {
             
        }

        public class TestSystem1 : TestSystemBase<ITestSystem1Server, ITestSystem1Client> {
             
        }

        [Test]
        public void TypeRegistryCanRegisterSyste() {
            var registry = new SystemTypeRegistry();
            registry.RegisterSystemFromConcreteType(typeof (TestSystem1));

            var registeredSystem = registry.GetSystemFromConcreteType(typeof(TestSystem1));
            registeredSystem.ServerInterfaceType.Should().Be(typeof (ITestSystem1Server));
            registeredSystem.ClientInterfaceType.Should().Be(typeof (ITestSystem1Client));

            registry.GetSystemFromServerInterfaceType(typeof(ITestSystem1Server)).Should().Be(registeredSystem);
            registry.GetSystemFromClientInterfaceType(typeof(ITestSystem1Client)).Should().Be(registeredSystem);
        }
    }
}