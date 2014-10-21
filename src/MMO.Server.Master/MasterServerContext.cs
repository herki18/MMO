using Autofac;
using MMO.Base.Components.Systems;
using MMO.Base.Infrastructure;
using MMO.Server.Master.Systems;

namespace MMO.Server.Master {
    public class MasterServerContext : ServerContext {
        public MasterServerContext(ISerializer serializer) : base(serializer) {
            
        }
        
        protected override void Configure(ContainerBuilder builder) {
            SystemTypeRegistry.ScanAssembly(typeof(MasterServerContext).Assembly);
            PlayerSystemComponentMap.AutoMapAssembly(typeof(ILoginSystemServer).Assembly, typeof(ComponentInterfaceAttribute));

            foreach (var system in SystemTypeRegistry.RegisterdSystems) {
                builder.RegisterType(system.ConcreteType).AsSelf().InstancePerLifetimeScope();
            }
        }

        protected override void InitPlayerContext(PlayerContext playerContext) {
            playerContext.Systems.Create<LoginSystem>();
        }
    }
}