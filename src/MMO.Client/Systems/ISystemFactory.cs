using System;
using MMO.Base.Infrastructure;

namespace MMO.Client.Systems {
    public interface ISystemFactory {
        ISystemBase CreateSystem(Type interfaceType, Func<Type, object> proxyFactory, out Type concreteType);
    }
}