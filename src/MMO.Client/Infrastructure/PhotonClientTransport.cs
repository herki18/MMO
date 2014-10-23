using System.Collections.Generic;
using ExitGames.Client.Photon;
using log4net;
using MMO.Base.Infrastructure;

namespace MMO.Client.Infrastructure {
    public class PhotonClientTransport : ClientTransportBase, IPhotonPeerListener {
        private static readonly ILog Log = LogManager.GetLogger(typeof (PhotonClientTransport));
        private readonly PhotonPeer _peer;


        public PhotonClientTransport() {
            _peer = new PhotonPeer(this, ConnectionProtocol.Udp);

        }

        public void Connect(string serverAddress, string application) {
            _peer.Connect(serverAddress, application);
        }

        public override void Disconnect() {
            _peer.Disconnect();
        }

        public void Service() {
            _peer.Service();
        }

        protected override void SendOperationInternal(OperationCode code, Dictionary<byte, object> parameters) {
            _peer.OpCustom((byte) code, parameters, true);
        }


        void IPhotonPeerListener.DebugReturn(DebugLevel level, string message) {
            Log.DebugFormat("Debug Return: ({0}) {1}",level,message);
        }

        void IPhotonPeerListener.OnEvent(EventData eventData){
            HandleEvent((EventCode)eventData.Code, eventData.Parameters);
        }

        void IPhotonPeerListener.OnOperationResponse(OperationResponse operationResponse) {
            HandleSystemCallback((OperationCode)operationResponse.OperationCode, operationResponse.Parameters);
        }

        void IPhotonPeerListener.OnStatusChanged(StatusCode statusCode) {
            Log.DebugFormat("Status Changed: {0}", statusCode);

            var transportStatus = statusCode == StatusCode.Connect ? ClientTransportStatus.Connected : ClientTransportStatus.Disconnected;
            foreach (var listener in Listeners) {
                listener.TransportStatusChanged(transportStatus);
            }
        }
    }
}