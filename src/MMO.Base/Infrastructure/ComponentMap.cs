using System;
using System.Collections.Generic;

namespace MMO.Base.Infrastructure {
    public class ComponentMap {
        public readonly Dictionary<Type, MappedComponent> _typesToComponents;

        public MappedComponent[] Components { get; private set; }
        public MappedMethod[][] Methods { get; private set; }

        public ComponentMap() {
            _typesToComponents = new Dictionary<Type, MappedComponent>();
            Components = new MappedComponent[byte.MaxValue + 1];
            Methods = new MappedMethod[byte.MaxValue + 1][];
            
        }

        public MappedComponent MapComponent(Type componentType, byte componentId) {

            if (Components[componentId] != null) {
                throw new ArgumentException("Component already mapped", "componentId");
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