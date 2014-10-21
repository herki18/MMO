using System;

namespace MMO.Base.Infrastructure {
    public interface ISystemBase : IDisposable {
        Type ServerSystemInterfaceType { get; }
        Type ClientSystemInterfaceType { get; }
    }
}