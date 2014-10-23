using System;
using MMO.Base.Infrastructure;

namespace MMO.Client.Systems {
    public class ActivatorSystemFactory<TSystemBase> : ISystemFactory {
        private readonly SystemTypeRegistry _systemTypeRegistry;
        private readonly Action<TSystemBase, object> _initSystem;

        public ActivatorSystemFactory(SystemTypeRegistry systemTypeRegistry, Action<TSystemBase, object> initSystem) {
            _systemTypeRegistry = systemTypeRegistry;
            _initSystem = initSystem;
        }

        public ISystemBase CreateSystem(Type interfaceType, Func<Type, object> proxyFactory, out Type concreteType) {
            var registeredSystem = _systemTypeRegistry.GetSystemFromClientInterfaceType(interfaceType);
            var systemInstance = (ISystemBase) Activator.CreateInstance(registeredSystem.ConcreteType);

            var proxy = proxyFactory(registeredSystem.ServerInterfaceType);
            _initSystem((TSystemBase) systemInstance, proxy);

            concreteType = registeredSystem.ConcreteType;
            return systemInstance;
        }
    }
}