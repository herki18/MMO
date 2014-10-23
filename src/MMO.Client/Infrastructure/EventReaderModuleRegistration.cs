using System;
using System.Collections.Generic;
using MMO.Base.Infrastructure;

namespace MMO.Client.Infrastructure {
    public class EventReaderModuleRegistration {
        public EventCode Code { get; private set; }
        public Action<EventCode, Dictionary<byte, object>> Action { get; private set; }

        public EventReaderModuleRegistration(EventCode code, Action<EventCode, Dictionary<byte, object>> action) {
            Code = code;
            Action = action;
        }
    }
}