using System;
using MMO.Base.Infrastructure;

namespace MMO.Client.Console {
    public class ConsoleSystemBase<TServer,TClient> : ISystemBase<TServer,TClient>, IConsoleSystemBase {

        public Type ServerSystemInterfaceType { get; private set; }
        public Type ClientSystemInterfaceType { get; private set; }
        protected TServer Proxy;
        protected ConsoleContext Context;

        protected virtual void Awake()
        { }


        public void SetContext(ConsoleContext context, object proxy) {
            Context = context;
            Proxy = (TServer) proxy;
            Awake();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}