using System;
using System.Runtime.Remoting.Contexts;
using MMO.Base.Infrastructure;

namespace MMO.Server {
    public class ClientSystems {
        private readonly ClientContext _client;
        private readonly ServerContext _server;

        public ClientSystems(ClientContext client, ServerContext server) {
            _client = client;
            _server = server;
        }

        public TConcreteType Create<TConcreteType>() {
            throw new NotImplementedException();
        }

        public void Destroy<TConcreteType>() {
            throw new NotImplementedException();
        }

        public TConcreteType Get<TConcreteType>() {
            throw new NotImplementedException();
        }

        public ISystemBase GetByServerInterfaceId(byte serverInterfaceId) {
            throw new NotImplementedException();
        }
    }
}