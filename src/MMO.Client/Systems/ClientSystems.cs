using System;
using System.Collections.Generic;
using Castle.DynamicProxy;
using MMO.Base.Infrastructure;

namespace MMO.Client.Systems {
    public class ClientSystems : IDisposable {

        private readonly ISystemFactory _factory;
        private readonly ISystemOperationWriter _operationWriter;
        private readonly SystemInformation[] _systemsByInterfaceId;
        private readonly Dictionary<Type, SystemInformation> _concreteTypeToInfo;

        public ComponentMap ComponentMap { get; private set; }

        public ClientSystems(ComponentMap componentMap, ISystemFactory systemFactory, ISystemOperationWriter operationWriter) {
            ComponentMap = componentMap;
            _factory = systemFactory;
            _operationWriter = operationWriter;
            _concreteTypeToInfo = new Dictionary<Type, SystemInformation>();
            _systemsByInterfaceId = new SystemInformation[byte.MaxValue + 1];
        }

        public void Create(MappedComponent interfaceComponentType) {
            if (_systemsByInterfaceId[interfaceComponentType.Id] != null) {
                throw new ArgumentException(String.Format("System with interface type {0} has already been created", interfaceComponentType.Type.FullName), "interfaceComponentType");
            }

            _operationWriter.CreateSystem(interfaceComponentType);

            Type concreteType;
            var systemInstance = _factory.CreateSystem(interfaceComponentType.Type, CreateProxyFor, out concreteType);
            var systemInformation = new SystemInformation(interfaceComponentType, systemInstance, concreteType);

            _concreteTypeToInfo.Add(concreteType, systemInformation);
            _systemsByInterfaceId[interfaceComponentType.Id] = systemInformation;
        }

        private object CreateProxyFor(Type otherInterfaceComponenType) {
            return ComponentProxyGenerator.CreateInterfaceProxyWithoutTarget(
                otherInterfaceComponenType,
                new ProxyInterceptor(ComponentMap.GetComponent(otherInterfaceComponenType), _operationWriter));
        }

        public void Destroy(MappedComponent interfaceComponent) {
            if (_systemsByInterfaceId[interfaceComponent.Id] == null) {
                throw new ArgumentException(String.Format("System with interface type {0} is not created", interfaceComponent.Type.FullName), "interfaceComponent");
            }

            var system = _systemsByInterfaceId[interfaceComponent.Id];
            system.SystemInstance.Dispose();
            _operationWriter.DestroySystem(interfaceComponent);

            _systemsByInterfaceId[interfaceComponent.Id] = null;
            _concreteTypeToInfo.Remove(system.ConcreteType);
        }

        public ISystemBase GetByInterfaceId(byte interfaceId) {
            return _systemsByInterfaceId[interfaceId].SystemInstance;
        }

        public TConcreteType Get<TConcreteType>() where TConcreteType : class, ISystemBase {
            return (TConcreteType) _concreteTypeToInfo[typeof (TConcreteType)].SystemInstance;
        }

        public bool TryGet<TConcreteType>(out TConcreteType value) where TConcreteType : class, ISystemBase {
            SystemInformation information;
            if (!_concreteTypeToInfo.TryGetValue(typeof (TConcreteType), out information)) {
                value = null;
                return false;
            }

            value = (TConcreteType) information.SystemInstance;
            return true;
        }

        public bool Contains<TConcreteType>() where TConcreteType : class , ISystemBase{
            return _concreteTypeToInfo.ContainsKey(typeof (TConcreteType));
        }

        public void Dispose() {
            foreach (var system in _concreteTypeToInfo) {
                system.Value.SystemInstance.Dispose();
                _operationWriter.DestroySystem(system.Value.InterfaceType);
            }

            for (int i = 0; i < _systemsByInterfaceId.Length; i++) {
                _systemsByInterfaceId[i] = null;
            }
        }

        private class SystemInformation {
            public MappedComponent InterfaceType { get; private set; }
            public ISystemBase SystemInstance { get; private set; }
            public Type ConcreteType { get; private set; }

            public SystemInformation(MappedComponent interfaceType, ISystemBase systemInstance, Type concreteType) {
                InterfaceType = interfaceType;
                SystemInstance = systemInstance;
                ConcreteType = concreteType;
            }
        }

        private class ProxyInterceptor : IInterceptor {
            private readonly MappedComponent _proxyInterfaceComponent;
            private readonly ISystemOperationWriter _operationWriter;

            public ProxyInterceptor(MappedComponent proxyInterfaceComponent, ISystemOperationWriter operationWriter) {
                _proxyInterfaceComponent = proxyInterfaceComponent;
                _operationWriter = operationWriter;
            }

            public void Intercept(IInvocation invocation) {
                var method = _proxyInterfaceComponent.GetMethod(invocation.Method);
                _operationWriter.InvokeMethod(method, invocation.Arguments);
                // TODO: WHEN WE HAVE RPC RESPONSE
            }
        }
    }
}