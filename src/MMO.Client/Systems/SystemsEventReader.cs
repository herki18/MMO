using System.Collections.Generic;
using System.IO;
using MMO.Base.Infrastructure;
using MMO.Client.Infrastructure;

namespace MMO.Client.Systems {
    public class SystemsEventReader : IEventReaderModule {
        private readonly ISerializer _serializer;
        private readonly ClientSystems _systems;

        public SystemsEventReader(ISerializer serializer, ClientSystems systems) {
            _serializer = serializer;
            _systems = systems;
        }


        public IEnumerable<EventReaderModuleRegistration> GetRegistrations() {
            return new[] {
                new EventReaderModuleRegistration(EventCode.InvokeMethodOnSystem, InvokeMethodOnSystemEvent),
                new EventReaderModuleRegistration(EventCode.AddSystem, AddSystemEvent),
                new EventReaderModuleRegistration(EventCode.RemoveSystem, RemoveSystemEvent),
                new EventReaderModuleRegistration(EventCode.SyncSystemComponentMap, SyncSystemsComponentMap)
            };
        }

        private void InvokeMethodOnSystemEvent(EventCode code, Dictionary<byte, object> parameters) {
            var clientInterface = _systems.ComponentMap.Components[(byte) parameters[(byte) EventCodeParameter.ClientInterfaceTypeId]];
            var method = clientInterface.Methods[(byte) parameters[(byte) EventCodeParameter.MethodId]];

            object[] arguments;
            var argumentBytes = (byte[]) parameters[(byte) EventCodeParameter.ArgumentsBytes];

            using (var ms  =new MemoryStream(argumentBytes))
            using (var br = new BinaryReader(ms)) {
                arguments = _serializer.ReadArguments(br, method.ParameterTypes);
            }

            var system = _systems.GetByInterfaceId(clientInterface.Id);
            method.Invoke(system, arguments);
        }
        
        private void AddSystemEvent(EventCode code, Dictionary<byte, object> parameters) {
            var clientInterface = _systems.ComponentMap.Components[(byte)parameters[(byte)EventCodeParameter.ClientInterfaceTypeId]];
            _systems.Create(clientInterface);
        }
        
        private void RemoveSystemEvent(EventCode code, Dictionary<byte, object> parameters) {
            var clientInterface = _systems.ComponentMap.Components[(byte)parameters[(byte)EventCodeParameter.ClientInterfaceTypeId]];
            _systems.Destroy(clientInterface);
        }
        
        private void SyncSystemsComponentMap(EventCode code, Dictionary<byte, object> parameters) {
            var formatter = new ComponentMapBinaryFormatter();
            var componentMapBytes = (byte[]) parameters[(byte) EventCodeParameter.ComponentMapBytes];

            using (var ms = new MemoryStream(componentMapBytes)) 
            using (var br = new BinaryReader(ms)) {
                formatter.Load(br, _systems.ComponentMap);
            }
        }
    }
}