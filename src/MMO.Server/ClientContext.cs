using System;
using System.Collections.Generic;
using Autofac;
using MMO.Base.Infrastructure;

namespace MMO.Server {
    public  abstract class ClientContext : IDisposable{
        public ServerContext Application { get; private set; }
        public ClientSystems Systems { get; private set; }
        public ILifetimeScope ClientScope { get; private set; }
        public IServerTransport Transport { get; private set; }

        protected ClientContext() {
            throw new NotImplementedException();
        }

        public virtual void OnOperationRequest(OperationCode code, Dictionary<byte, object> parameters) {
            throw new NotImplementedException();
        }

        protected void InvokeMethodOnSyste(byte serverInterfaceComponentId, Dictionary<byte, object> parameters) {
            throw new NotImplementedException();
        }

        internal void OnDisconnect() {
            throw new NotImplementedException();
        }

        public void Dispose() {
            throw new NotImplementedException();
        }
    }
}