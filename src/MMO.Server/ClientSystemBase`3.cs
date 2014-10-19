using System;
using MMO.Base.Infrastructure;

namespace MMO.Server {
    public class ClientSystemBase<TContextType, TServerInterface, TClientInterface> : ISystemBase<TServerInterface, TClientInterface>, IInternalSystem
        where TContextType : ClientContext {

        public Type ServerSystemInterfaceType { get; private set; }
        public Type ClientSystemInterfaceType { get; private set; }

        protected TContextType Context;
        protected TClientInterface Proxy;

        public void Init(ClientContext client, object proxy) {
            throw new NotImplementedException();
        }
    }
}