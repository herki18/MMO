using System.Reflection;

namespace MMO.Base.Infrastructure {
    public class MappedMethod {
        public MappedComponent Component { get; private set; }
        public MethodInfo MethodInfo { get; private set; }
        public byte Id { get; private set; }

        public MappedMethod(MappedComponent component, MethodInfo methodInfo, byte id) {
            Component = component;
            MethodInfo = methodInfo;
            Id = id;
        }
    }
}