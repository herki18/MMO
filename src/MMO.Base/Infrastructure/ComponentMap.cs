using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MMO.Base.Infrastructure {
    public class ComponentMap {
        private byte _nextAutoMappedComponentId;

        private readonly Dictionary<Type, MappedComponent> _typesToComponents;

        public byte ReservedComponentIdLimit { get; private set; }
        public MappedComponent[] Components { get; private set; }
        public MappedMethod[][] Methods { get; private set; }
        public IEnumerable<MappedComponent> AllComponents { get { return _typesToComponents.Values; } }
        public int ComponentCount { get { return _typesToComponents.Count; } }

        public ComponentMap() : this(0) {}

        public ComponentMap(byte reservedComponentIdLimit) {
            ReservedComponentIdLimit = reservedComponentIdLimit;
            _typesToComponents = new Dictionary<Type, MappedComponent>();
            Components = new MappedComponent[byte.MaxValue + 1];
            Methods = new MappedMethod[byte.MaxValue + 1][];

            _nextAutoMappedComponentId = reservedComponentIdLimit;
        }

        public MappedComponent AutoMapComponent(Type componentType) {
            var mappedComponent = MapComponent(componentType, _nextAutoMappedComponentId);
            _nextAutoMappedComponentId++;

            mappedComponent.AutoMapMethods();

            return mappedComponent;
        }

        public void AutoMapAssembly(Assembly assemblyToMap, Type attributeSelector) {
            foreach (var type in assemblyToMap.GetTypes()) {
                if (type.GetCustomAttributes(attributeSelector, false).Any()) {
                    AutoMapComponent(type);
                }
            }
        }

        public void AutoMapCurrentAppDomain(Type attributeSelector) {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                AutoMapAssembly(assembly, attributeSelector);
            }
        }

        public MappedComponent MapComponent(Type componentType, byte componentId) {

            if (Components[componentId] != null) {
                throw new ArgumentException("Component already mapped", "componentId");
            }

            if (componentId < ReservedComponentIdLimit) {
                throw new ArgumentException("Id passed reserved component id limit", "componentId");
            }
            
            var component = new MappedComponent(componentType, componentId);
            _typesToComponents.Add(componentType, component);
            Components[componentId] = component;
            Methods[componentId] = component.Methods;
            return component;
        }

        public MappedComponent GetComponent(Type componentType) {
            return _typesToComponents[componentType];
        }
    }
}