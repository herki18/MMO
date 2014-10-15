using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MMO.Base.Infrastructure {
    public class MappedComponent {
        private readonly Dictionary<MethodInfo, MappedMethod> _methodInfoMethods;
        private byte _nextAutoMapMethodId;

        public Type Type { get; private set; }
        public byte Id { get; private set; }
        public MappedMethod[] Methods { get; set; }

        public MappedComponent(Type type, byte id) {
            Type = type;
            Id = id;
            Methods = new MappedMethod[byte.MaxValue+1];
            _methodInfoMethods = new Dictionary<MethodInfo, MappedMethod>();
            _nextAutoMapMethodId = 0;
        }

        public void AutoMapMethods() {
            foreach (var method in Type.GetMethods(BindingFlags.Instance | BindingFlags.Public)) {
                MapMethod(method, _nextAutoMapMethodId);
                _nextAutoMapMethodId++;
            }
        }

        public MappedMethod MapMethod(MethodInfo methodInfo, byte id) {
            if (Methods[id] != null) {
                throw new ArgumentException("Method id already mapped", "id");
            }

            if (methodInfo.DeclaringType != Type) {
                throw new ArgumentException("Method must be declare on component type", "methodInfo");
            }

            var method = new MappedMethod(this, methodInfo, id);
            _methodInfoMethods.Add(methodInfo, method);

            Methods[id] = method;

            return method;
        }

        public MappedMethod GetMethod(MethodInfo method) {
            return _methodInfoMethods[method];
        }
    }
}