using System.Collections.Generic;
using MMO.Base.Infrastructure;

namespace MMO.Server {
    public interface IServerTransport {
        void SendData(Event @event);
        void SendOperationResponse(OperationCode code, Dictionary<byte, object> parameters);
        
        void Disconnect();
    }
}