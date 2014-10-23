using System;
using System.Collections.Generic;
using MMO.Base.Infrastructure;

namespace MMO.Client.Infrastructure {
    public interface IClientTransport {
        void SendOperation(OperationCode code, Dictionary<byte, object> parameters);
        void SendOperation(OperationCode code, Dictionary<byte, object> parameters, Action<OperationCode, Dictionary<byte, object>> onResponse);
        void AddEventReader(IEventReaderModule eventReader);
        void AddListener(IClientTransportListener listener);

        void Disconnect();
    }
}