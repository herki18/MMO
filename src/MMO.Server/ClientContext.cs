using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using MMO.Base.Infrastructure;

namespace MMO.Server {
    public  abstract class ClientContext : IDisposable{
        public ServerContext Application { get; private set; }
        public ClientSystems Systems { get; private set; }
        public ILifetimeScope ClientScope { get; private set; }
        public IServerTransport Transport { get; private set; }
        public ComponentMap SystemsComponentMap { get; private set; }

        protected ClientContext(ServerContext application, ComponentMap systemsComponentMap, IServerTransport transport) {
            Application = application;
            Systems = new ClientSystems(this);
            ClientScope = application.Container.BeginLifetimeScope(this);
            SystemsComponentMap = systemsComponentMap;
            Transport = transport;

            transport.SendData(Application.Events.SyncComponentMap(EventCode.SyncSystemComponentMap, systemsComponentMap));
        }

        public virtual void OnOperationRequest(OperationCode code, Dictionary<byte, object> parameters) {
            InvokeMethodOnSystem((byte)code, parameters);
        }

        protected void InvokeMethodOnSystem(byte serverInterfaceComponentId, Dictionary<byte, object> parameters) {
            var serverInterfaceComponent = SystemsComponentMap.Components[serverInterfaceComponentId];
            var methodId = (byte) parameters[(byte) OperationParameter.MethodId];
            var method = serverInterfaceComponent.Methods[methodId];
            var argumentByte = (byte[]) parameters[(byte) OperationParameter.ArgumentBytes];

            object[] arguments;

            using (var ms = new MemoryStream(argumentByte))
            using (var br = new BinaryReader(ms)) {
                arguments = Application.Serializer.ReadArguments(br, method.ParameterTypes);
            }

            var systemObject = Systems.GetByServerInterfaceId(serverInterfaceComponentId);
            var result = method.Invoke(systemObject, arguments);

            if (method.ReturnType == MappedMethodReturnType.Void)
                return;

            if (result == null) {
                throw new NullReferenceException("Please return an IRpcResponse or IRpcResponse<T> for non-void system method");
            }

            var systemInvokeId = (byte) parameters[(byte) OperationParameter.SystemInvokeId];
            var responseParameters = new Dictionary<byte, object>();
            responseParameters[(byte) OperationParameter.SystemInvokeId] = systemInvokeId;
            responseParameters[(byte) OperationParameter.ResponseIsValid] = result.IsValid;
            responseParameters[(byte) OperationParameter.ResponseOperationErrors] = result.OperationErrors;

            if (method.ReturnType == MappedMethodReturnType.Response) {
                Transport.SendOperationResponse(OperationCode.SendSystemResponse, parameters);
            }
            else {
                using (var ms = new MemoryStream())
                using(var bw = new BinaryWriter(ms)){
                    Application.Serializer.WriteObject(bw, method.ResultType, result.UntypedResult);
                    responseParameters[(byte) OperationParameter.ResultBytes] = ms.ToArray();
                    Transport.SendOperationResponse(OperationCode.SendSystemResponse, parameters);
                }
            }

        }

        internal void OnDisconnect() {
            Systems.Dispose();
            ClientScope.Dispose();
        }

        public void Dispose() {
            Transport.Disconnect();
        }
    }
}