using System;
using MMO.Base.Infrastructure;

namespace MMO.Client.Systems {
    public interface ISystemOperationWriter {
        void InvokeMethod(MappedMethod method, object[] arguments);
        void InvokeMethod(MappedMethod method, object[] arguments, Action<IRpcResponse> onResponse);

        void CreateSystem(MappedComponent interfaceComponent);
        void DestroySystem(MappedComponent interafaceComponent);
    }
}