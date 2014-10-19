using System;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;

namespace MMO.Server {
    public class MMOPeer : PeerBase{
        public MMOPeer(InitRequest initRequest) : base(initRequest) {
            throw new NotImplementedException();
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters) {
            throw new NotImplementedException();
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail) {
            throw new NotImplementedException();
        }
    }
}