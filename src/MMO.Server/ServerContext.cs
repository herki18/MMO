using System;
using System.Runtime.CompilerServices;
using Autofac;
using Autofac.Core;
using MMO.Base.Infrastructure;

namespace MMO.Server {
    public abstract class ServerContext {
        public ComponentMap PlayerSystemComponentMap { get; private set; }
        public ComponentMap ActorsComponentMap { get; private set; }

        public SystemTypeRegistry SystemTypeRegistry { get; private set; }
        
        public IContainer Container { get; private set; }
        public ISerializer Serializer { get; private set; }
        public EventWriter Events { get; set; }

        protected ServerContext(ISerializer serializer) {
            PlayerSystemComponentMap = new ComponentMap();
            ActorsComponentMap = new ComponentMap();
            SystemTypeRegistry = new SystemTypeRegistry();
            Serializer = serializer;
            Events = new EventWriter(serializer);

            var builder = new ContainerBuilder();

            Configure(builder);

            Container = builder.Build();
        }

        protected abstract void Configure(ContainerBuilder builder);

        protected internal virtual void InitPlayerContext(PlayerContext playerContext) {
            
        }
    }
}