using System;
using System.Collections.Generic;
using MMO.Base.Infrastructure;

namespace MMO.Client.Infrastructure {
    public abstract class ClientTransportBase : IClientTransport {
        private readonly CallbackByteMap<Action<OperationCode, Dictionary<byte, object>>> _callbacks;
        private readonly Action<EventCode, Dictionary<byte, object>>[] _eventHandlers;
        protected readonly HashSet<IClientTransportListener> Listeners;

        protected ClientTransportBase() {
            _callbacks = new CallbackByteMap<Action<OperationCode, Dictionary<byte, object>>>();
            _eventHandlers = new Action<EventCode, Dictionary<byte, object>>[byte.MaxValue + 1];
            Listeners = new HashSet<IClientTransportListener>();
        }

        public void AddEventReader(IEventReaderModule eventReader) {
            foreach (var registration in eventReader.GetRegistrations()) {
                if (_eventHandlers[(int) registration.Code] != null) {
                    var oldHandler = _eventHandlers[(int) registration.Code];
                    var registration1 = registration;
                    _eventHandlers[(int) registration.Code] = (code, parameters) => {
                        oldHandler(code, parameters);
                        registration1.Action(code, parameters);
                    };
                }
                else {
                    _eventHandlers[(int) registration.Code] = registration.Action;
                } 
            }
        }

        public void SendOperation(OperationCode code, Dictionary<byte, object> parameters) {
            SendOperationInternal(code, parameters);
        }

        public void SendOperation(OperationCode code, Dictionary<byte, object> parameters, Action<OperationCode, Dictionary<byte, object>> onResponse) {
            parameters[(byte) OperationParameter.SystemInvokeId] = _callbacks.RegisterCallback(onResponse);
            SendOperationInternal(code, parameters);
        }

        public void AddListener(IClientTransportListener listener) {
            Listeners.Add(listener);
        }

        public virtual void Disconnect() { }

        protected void HandleSystemCallback(OperationCode code, Dictionary<byte, object> parameters) {
            if (code != OperationCode.SendSystemResponse) {
                throw new ArgumentException(string.Format("Code {0} is not valid for handling responses", code), "code");
            }

            var methodInvokeId = (byte)parameters[(byte) OperationParameter.SystemInvokeId];
            var callback = _callbacks.GetCallback(methodInvokeId);
            callback(code, parameters);
        }

        protected void HandleEvent(EventCode code, Dictionary<byte, object> parameters) {
            var handler = _eventHandlers[(byte) code];
            if (handler == null) {
                throw new ArgumentException(string.Format("Handler for event coe {0} was not registered", code), "code");
            }

            handler(code, parameters);
        }

        protected abstract void SendOperationInternal(OperationCode code, Dictionary<byte, object> parameters);

    }
}