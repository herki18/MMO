using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MMO.Base.Infrastructure {
    public class SystemTypeRegistry {
        private readonly Dictionary<Type, RegisterdSystem> _concreteTypeToSystem;
        private readonly Dictionary<Type, RegisterdSystem> _serverInterfaceTypeToSystem;
        private readonly Dictionary<Type, RegisterdSystem> _clientInterfaceTypeToSystem;
        public IEnumerable<RegisterdSystem> RegisterdSystems { get { return _concreteTypeToSystem.Values; } } 

        public SystemTypeRegistry() {
            _concreteTypeToSystem = new Dictionary<Type, RegisterdSystem>();
            _serverInterfaceTypeToSystem = new Dictionary<Type, RegisterdSystem>();
            _clientInterfaceTypeToSystem = new Dictionary<Type, RegisterdSystem>();
        }

        public void RegisterSystemFromConcreteType(Type concreteType) {
            foreach (var inter in concreteType.GetInterfaces()) {
                if (!inter.IsGenericType || inter.GetGenericTypeDefinition() != typeof (ISystemBase<,>)) {
                    continue;
                }

                var genericArguments = inter.GetGenericArguments();
                var serverInterfaceType = genericArguments[0];
                var clientInterfaceType = genericArguments[1];

                var system = new RegisterdSystem(concreteType, serverInterfaceType, clientInterfaceType);
                _concreteTypeToSystem.Add(concreteType, system);
                _serverInterfaceTypeToSystem.Add(serverInterfaceType, system);
                _clientInterfaceTypeToSystem.Add(clientInterfaceType, system);

                break;
            }
        }

        public void ScanAssembly(Assembly assembly) {
            ScanAssembly(assembly, null);
        }

        public void ScanAssembly(Assembly assembly, Type attributeFilter) {
            foreach (var type in assembly.GetTypes()) {
                if (!type.IsClass || type.IsAbstract || !typeof (ISystemBase).IsAssignableFrom(type)) {
                    continue;
                }

                if (attributeFilter != null && !type.GetCustomAttributes(attributeFilter, false).Any()) {
                    continue;
                }

                RegisterSystemFromConcreteType(type);
            }
        }

        public void ScanAppDomain() {
            ScanAppDomain(null);
        }

        public void ScanAppDomain(Type attributeFilter) {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                ScanAssembly(assembly, attributeFilter);
            }
        }

        public RegisterdSystem GetSystemFromConcreteType(Type concreteType) {
            return _concreteTypeToSystem[concreteType];
        }

        public bool TryGetSystemFromConcreteType(Type concreteType, out RegisterdSystem registerdSystem) {
            return _concreteTypeToSystem.TryGetValue(concreteType, out registerdSystem);
        }

        public RegisterdSystem GetSystemFromServerInterfaceType(Type serverInterfaceType) {
            return _serverInterfaceTypeToSystem[serverInterfaceType];
        }

        public bool TryGetSystemFromServerInterfaceType(Type serverInterfaceType, out RegisterdSystem registerdSystem) {
            return _serverInterfaceTypeToSystem.TryGetValue(serverInterfaceType, out registerdSystem);
        }

        public RegisterdSystem GetSystemFromClientInterfaceType(Type clientInterfaceType) {
            return _clientInterfaceTypeToSystem[clientInterfaceType];
        }

        public bool TryGetSystemFromClientInterfaceType(Type clientInterfaceType, out RegisterdSystem registerdSystem){
            return _clientInterfaceTypeToSystem.TryGetValue(clientInterfaceType, out registerdSystem);
        }
    }
}