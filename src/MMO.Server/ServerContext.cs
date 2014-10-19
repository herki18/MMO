using System;
using Autofac;
using MMO.Base.Infrastructure;

namespace MMO.Server {
    public class ServerContext {
        public ComponentMap PlayerSystemComponentMap { get; private set; }
        public ComponentMap ActorsComponentMap { get; private set; }
        public SystemTypeRegistry SystemTypeRegistry { get; private set; }
        public IContainer Container { get; private set; }

        public ServerContext() {
            throw new NotImplementedException();
        }
    }
}