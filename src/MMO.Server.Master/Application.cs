using Photon.SocketServer;

namespace MMO.Server.Master
{
    public class Application : ApplicationBase
    {
        protected override PeerBase CreatePeer(InitRequest initRequest) {
            return new Peer(initRequest);
        }

        protected override void Setup() {
        }

        protected override void TearDown() {
        }
    }
}
