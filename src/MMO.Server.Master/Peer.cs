using System.Collections.Generic;
using System.Configuration;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;

namespace MMO.Server.Master
{
    public class Peer : PeerBase
    {
        public Peer(InitRequest initRequest) : base(initRequest) {
            SendEvent(new EventData(0, new Dictionary<byte, object> {
                {0, ConfigurationManager.AppSettings["RegionServers"]}
            }), new SendParameters{Unreliable = false});
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters) {
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail) {
        }
    }
}
