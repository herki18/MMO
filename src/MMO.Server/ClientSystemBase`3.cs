using System;
using MMO.Base.Infrastructure;

namespace MMO.Server {
    public abstract class ClientSystemBase<TContextType, TServerInterface, TClientInterface> : ISystemBase<TServerInterface, TClientInterface>, IInternalSystem
        where TContextType : ClientContext {

        public Type ServerSystemInterfaceType { get; private set; }
        public Type ClientSystemInterfaceType { get; private set; }

        protected TContextType Context;
        protected TClientInterface Proxy;

        public void Init(ClientContext client, object proxy) {
            Context = (TContextType) client;
            Proxy = (TClientInterface) proxy;

            Awake();
        }

        protected virtual void Awake()
        { }

        public virtual void Dispose()
        { }
    }
}