using MMO.Base.Infrastructure;
using Photon.SocketServer;

namespace MMO.Server.Master
{
    public class Application : ApplicationBase {
        private readonly MasterServerContext _application;

        public Application() {
            _application = new MasterServerContext(new SimpleSerializer());
        }

        protected override PeerBase CreatePeer(InitRequest initRequest) {
            return new MMOPeer(_application, initRequest);
        }

        protected override void Setup() {
        }

        protected override void TearDown() {
        }
    }
}
