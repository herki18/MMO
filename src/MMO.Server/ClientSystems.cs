using System;
using System.Collections.Generic;
using Autofac;
using Castle.DynamicProxy;
using MMO.Base.Infrastructure;

namespace MMO.Server {
    public class ClientSystems : IDisposable{
        private readonly ClientContext _client;
        private readonly ISystemBase[] _systems;
        private readonly Dictionary<Type, ISystemBase> _concreteTypeSystems;
 
        public ClientSystems(ClientContext client) {
            _client = client;
            // Maybe +1 
            _systems = new ISystemBase[byte.MaxValue];
            _concreteTypeSystems = new Dictionary<Type, ISystemBase>();
        }

        public TConcreteType Create<TConcreteType>() where TConcreteType : ISystemBase {
            var system = _client.ClientScope.Resolve<TConcreteType>();
            var serverComponent = _client.SystemsComponentMap.GetComponent(system.ServerSystemInterfaceType);
            var clientComponent = _client.SystemsComponentMap.GetComponent(system.ClientSystemInterfaceType);

            if (_systems[serverComponent.Id] != null) {
                throw new InvalidOperationException(string.Format("Cannot add {0} system twice!", typeof(TConcreteType).FullName));
            }

            var clientProxy = ComponentProxyGenerator.CreateInterfaceProxyWithoutTarget(
                system.ClientSystemInterfaceType,
                new ClientInterfaceInterceptor(clientComponent, _client.Transport, _client.Application.Events));

            _client.Transport.SendData(_client.Application.Events.AddSystem(clientComponent));

            _systems[serverComponent.Id] = system;
            _concreteTypeSystems.Add(typeof(TConcreteType), system);

            ((IInternalSystem)system).Init(_client, clientProxy);

            return system;
        }

        public void Destroy<TConcreteType>() {
            ISystemBase system;
            if (!_concreteTypeSystems.TryGetValue(typeof (TConcreteType), out system)) {
                throw new InvalidOperationException(string.Format("System {0} was not found", typeof(TConcreteType).FullName));
            }

            var serverComponent = _client.SystemsComponentMap.GetComponent(system.ServerSystemInterfaceType);
            var clientComponent = _client.SystemsComponentMap.GetComponent(system.ClientSystemInterfaceType);

            system.Dispose();
            _client.Transport.SendData(_client.Application.Events.RemoveSystem(clientComponent));
            _systems[serverComponent.Id] = null;
            _concreteTypeSystems.Remove(typeof (TConcreteType));

        }

        public TConcreteType Get<TConcreteType>() {
            return (TConcreteType) _concreteTypeSystems[typeof (TConcreteType)];
        }

        public ISystemBase GetByServerInterfaceId(byte serverInterfaceId) {
            return _systems[serverInterfaceId];
        }

        public void Dispose() {
            foreach (var system in _concreteTypeSystems) {
                system.Value.Dispose();
            }
        }

        private class ClientInterfaceInterceptor : IInterceptor {
            private readonly MappedComponent _clientInterfaceComponent;
            private readonly IServerTransport _transport;
            private readonly EventWriter _eventWriter;

            public ClientInterfaceInterceptor(MappedComponent clientInterfaceComponent, IServerTransport transport, EventWriter eventWriter) {
                _clientInterfaceComponent = clientInterfaceComponent;
                _transport = transport;
                _eventWriter = eventWriter;
            }

            public void Intercept(IInvocation invocation) {
                _transport.SendData(_eventWriter.InvokeMethodOnSystem(
                    _clientInterfaceComponent.Id, 
                    _clientInterfaceComponent.GetMethod(invocation.Method), 
                    invocation.Arguments));
            }
        }
    }
}