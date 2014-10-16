using System;

namespace MMO.Base.Infrastructure {
    public class RegisterdSystem {
        public Type ConcreteType { get; private set; }
        public Type ServerInterfaceType { get; private set; }
        public Type ClientInterfaceType { get; private set; }

        public RegisterdSystem(Type concreteType, Type serverInterfaceType, Type clientInterfaceType) {
            ConcreteType = concreteType;
            ServerInterfaceType = serverInterfaceType;
            ClientInterfaceType = clientInterfaceType;
        }
    }
}