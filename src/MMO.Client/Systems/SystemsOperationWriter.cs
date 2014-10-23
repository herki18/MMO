using System;
using System.Collections.Generic;
using System.IO;
using MMO.Base.Infrastructure;
using MMO.Client.Infrastructure;

namespace MMO.Client.Systems {
    public class SystemsOperationWriter : ISystemOperationWriter {
        private readonly ISerializer _serializer;
        private readonly IClientTransport _transport;

        public SystemsOperationWriter(ISerializer serializer, IClientTransport transport) {
            _serializer = serializer;
            _transport = transport;
        }

        public void InvokeMethod(MappedMethod method, object[] arguments) {
            var parameters = new Dictionary<byte, object>();
            SetParameters(parameters, method, arguments);
            _transport.SendOperation((OperationCode)method.Component.Id, parameters);
        }

        public void InvokeMethod(MappedMethod method, object[] arguments, Action<IRpcResponse> onResponse) {
            var parameters = new Dictionary<byte, object>();
            SetParameters(parameters, method, arguments);
            _transport.SendOperation((OperationCode)method.Component.Id, parameters, ParseResponse(method, onResponse));
        }

        public void CreateSystem(MappedComponent interfaceComponent) {
            throw new NotImplementedException();
        }

        public void DestroySystem(MappedComponent interafaceComponent) {
            throw new NotImplementedException();
        }

        private Action<OperationCode, Dictionary<byte, object>> ParseResponse(MappedMethod method, Action<IRpcResponse> onResponse) {
            return (code, parameters) => {
                var isValid = parameters[(byte) OperationParameter.ResponseIsValid];
                var operationErrors = parameters[(byte) OperationParameter.ResponseOperationErrors];
                object resultObject;

                if (method.ReturnType == MappedMethodReturnType.ResponseWithResult) {
                    var resultBytes = (byte[])parameters[(byte) OperationParameter.ResultBytes];
                    using (var ms = new MemoryStream())
                    using (var br = new BinaryReader(ms)) {
                        resultObject = _serializer.ReadObject(br, method.ResultType);

                        // TODO: Create new RPc Response Object and invoke onResponse
                    }
                }
                else if(method.ReturnType == MappedMethodReturnType.Response) {
                    // TODO: Create new RPc Response Object and invoke onResponse
                }
                else {
                    throw new InvalidOperationException(string.Format("Cannot accept return for method without return value: {0}.{1}", 
                        method.MethodInfo.DeclaringType.FullName,
                        method.MethodInfo.Name));
                }
            };
        }

        private void SetParameters(Dictionary<byte, object> parameters, MappedMethod method, object[] arguments) {
            byte[] argumentBytes;

            using (var ms = new MemoryStream()) 
            using (var bw = new BinaryWriter(ms)) {
                _serializer.WriteArguments(bw, method.ParameterTypes, arguments);
                argumentBytes = ms.ToArray();
            }

            parameters[(byte) OperationParameter.ArgumentBytes] = argumentBytes;
            parameters[(byte) OperationParameter.MethodId] = method.Id;

        }
    }
}