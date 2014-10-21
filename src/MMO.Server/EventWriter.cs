using System.Collections.Generic;
using System.IO;
using MMO.Base.Infrastructure;

namespace MMO.Server {
    public class EventWriter {
        private readonly ISerializer _serializer;

        public EventWriter(ISerializer serializer) {
            _serializer = serializer;
        }

        public Event SyncComponentMap(EventCode eventCode, ComponentMap componentMap) {
            using (var ms = new MemoryStream())
            using(var bw = new BinaryWriter(ms)) {
                var formatter = new ComponentMapBinaryFormatter();
                formatter.Save(bw, componentMap);
                
                var parameters = new Dictionary<byte, object> {
                    {(byte) EventCodeParameter.ComponentMapBytes, ms.ToArray()}
                };

                return Event.FromDictionary(eventCode, parameters, Event.Reliable);
            }
        }

        public Event AddSystem(MappedComponent clientInterfaceComponent) {
            var parameters = new Dictionary<byte, object> {
                {(byte) EventCodeParameter.ClientInterfaceTypeId, clientInterfaceComponent.Id}
            };

            return Event.FromDictionary(EventCode.AddSystem, parameters, Event.Reliable);
        }

        public Event RemoveSystem(MappedComponent clientInterfaceComponent) {
            var parameters = new Dictionary<byte, object> {
                {(byte) EventCodeParameter.ClientInterfaceTypeId, clientInterfaceComponent.Id}
            };

            return Event.FromDictionary(EventCode.RemoveSystem, parameters, Event.Reliable);
        }

        public Event InvokeMethodOnSystem(byte clientInterfaceComponentId, MappedMethod method, object[] arguments) {
            byte[] argumentsBytes;

            using (var ms = new MemoryStream())
            using(var bw = new BinaryWriter(ms)){
                _serializer.WriteArguments(bw, method.ParameterTypes, arguments);
                argumentsBytes = ms.ToArray();
            }

            var parameters = new Dictionary<byte, object> {
                {(byte) EventCodeParameter.ClientInterfaceTypeId, clientInterfaceComponentId},
                {(byte)EventCodeParameter.MethodId, method.Id},
                {(byte)EventCodeParameter.ArgumentsBytes, argumentsBytes}
            };

            return Event.FromDictionary(EventCode.InvokeMethodOnSystem, parameters, Event.Reliable);
        }
    }
}