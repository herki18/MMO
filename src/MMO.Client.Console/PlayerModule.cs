using System.Collections.Generic;
using MMO.Base.Infrastructure;
using MMO.Client.Infrastructure;

namespace MMO.Client.Console {
    public class PlayerModule : IEventReaderModule {
        private readonly ConsoleContext _context;
        private readonly IClientTransport _transport;

        public PlayerModule(ConsoleContext context, IClientTransport transport) {
            _context = context;
            _transport = transport;
            _transport.AddEventReader(this);
        }

        public IEnumerable<EventReaderModuleRegistration> GetRegistrations() {
            yield break;
        }

        public void InitPlayer() {
            var parameters = new Dictionary<byte, object>();
            parameters[(byte) OperationParameter.ContextType] = ContextType.Player;
            _transport.SendOperation(OperationCode.InitContext, parameters);
        }
    }
}