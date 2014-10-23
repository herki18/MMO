using System;
using System.Collections.Generic;
using MMO.Base.Infrastructure;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using Serilog;

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
                Log.Debug("ClientContext is null");
                if (operationCode != OperationCode.InitContext) {
                    Log.Error("Operation code {OperationCode} is not supported", operationCode);
                    throw new NotImplementedException(string.Format("Operation code {0} is not supported", operationCode));
                }

                var contextType = (ContextType) operationRequest.Parameters[(byte) OperationParameter.ContextType];
                if (contextType == ContextType.Player) {
                    Log.Debug("Creating new PlayerContext");
                    _clientContext = new PlayerContext(_application, this);
                }
                else {
                    Log.Error("Context type {ContextType} is not valid", contextType);
                    throw new ArgumentException(string.Format("Context type {0} is not valid", contextType));
                }
            }
            else {
                Log.Debug("OnOperationRequest {OperationCode} and {OperationParameters}", operationCode, operationRequest.Parameters);
                _clientContext.OnOperationRequest(operationCode, operationRequest.Parameters);
            }
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail) {
            Log.Debug("OnDisconnect {ReasonCode} and {ReasonDetail}", reasonCode, reasonDetail);
            _clientContext.OnDisconnect();
        }

        #region IServerTransport

        public void SendData(Event @event) {
            Log.Debug("SendData: {EventData} and {SendParameters}", @event.EventData, @event.SendParameters);
            SendEvent(@event.EventData, @event.SendParameters);
        }

        public void SendOperationResponse(OperationCode code, Dictionary<byte, object> parameters)
        {
            Log.Debug("SendOperationResponse: {Code} and {Parameters}", code, parameters);
            SendOperationResponse(new OperationResponse((byte)code, parameters), Event.Reliable);
        }

        #endregion
    }
}