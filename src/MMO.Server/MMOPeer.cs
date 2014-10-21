using System;
using System.Collections.Generic;
using MMO.Base.Infrastructure;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;

namespace MMO.Server {
    public class MMOPeer : PeerBase, IServerTransport {
        private readonly ServerContext _application;
        private ClientContext _clientContext;

        public MMOPeer(ServerContext application, InitRequest initRequest) : base(initRequest) {
            _application = application;
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters) {
            var operationCode = (OperationCode) operationRequest.OperationCode;
            if (_clientContext == null) {
                
                if (operationCode != OperationCode.InitContext) {
                    throw new NotImplementedException(string.Format("Operation code {0} is not supported", operationCode));
                }

                var contextType = (ContextType) operationRequest.Parameters[(byte) OperationParameter.ContextType];
                if (contextType == ContextType.Player) {
                    _clientContext = new PlayerContext(_application, this);
                }
                else {
                    throw new ArgumentException(string.Format("Context type {0} is not valid", contextType));
                }
            }
            else {
                _clientContext.OnOperationRequest(operationCode, operationRequest.Parameters);
            }
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail) {
            _clientContext.OnDisconnect();
        }

        #region IServerTransport

        public void SendData(Event @event) {
            SendEvent(@event.EventData, @event.SendParameters);
        }

        public void SendOperationResponse(OperationCode code, Dictionary<byte, object> parameters)
        {
            SendOperationResponse(new OperationResponse((byte)code, parameters), Event.Reliable);
        }

        #endregion
    }
}