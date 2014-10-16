using System;

namespace MMO.Base.Infrastructure {
    public interface ISystemBase {
        Type ServerSystemInterfaceType { get; }
        Type ClientSystemInterfaceType { get; }
    }
}