using System.Collections.Generic;
using MMO.Base.Infrastructure;
using Photon.SocketServer;

namespace MMO.Server {
    public class Event {
        public IEventData EventData { get; private set; }
        public SendParameters SendParameters { get; private set; }

        public static Event FromDictionary(EventCode code, Dictionary<byte, object> data, SendParameters sendParameters) {
            return new Event {
                EventData =  new EventData((byte)code, sendParameters),
                SendParameters = sendParameters
            };
        }
    }
}