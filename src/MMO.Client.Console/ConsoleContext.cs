using MMO.Base.Infrastructure;
using MMO.Client.Infrastructure;
using MMO.Client.Systems;

namespace MMO.Client.Console {
    public class ConsoleContext : IClientTransportListener{
        private readonly IClientTransport _transport;
        private readonly PlayerModule _playerModule;

        public ClientSystems Systems { get; private set; }
        public SystemTypeRegistry TypeRegistry { get; private set; }

        public ConsoleContext(ISerializer serializer, IClientTransport transport) {
            _transport = transport;
            _playerModule = new PlayerModule(this, _transport);

            // Create Systems
            TypeRegistry = new SystemTypeRegistry();
            var systemFactory = new ActivatorSystemFactory<IConsoleSystemBase>(TypeRegistry, InitSystem);
            Systems = new ClientSystems(new ComponentMap(), systemFactory, new SystemsOperationWriter(serializer, transport));
            transport.AddEventReader(new SystemsEventReader(serializer, Systems));


            // Subscribe to transport
            _transport.AddListener(this);
        }

        public void TransportStatusChanged(ClientTransportStatus status) {
            if (status != ClientTransportStatus.Connected) {
                return;
            }

            _playerModule.InitPlayer();
        }

        private void InitSystem(IConsoleSystemBase system, object proxy) {
            system.SetContext(this, proxy);
        }
    }
}