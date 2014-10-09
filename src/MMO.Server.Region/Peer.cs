using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;

namespace MMO.Server.Region
{
    public class Peer : PeerBase
    {
        public Peer(InitRequest initRequest) : base(initRequest) { }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
        }
    }
}
