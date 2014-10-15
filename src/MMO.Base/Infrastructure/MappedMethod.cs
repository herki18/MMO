using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MMO.Base.Infrastructure {
    public class MappedMethod {
        public MappedComponent Component { get; private set; }
        public MethodInfo MethodInfo { get; private set; }
        public byte Id { get; private set; }

        public Func<object, object[], IRpcResponse> Invoke { get; private set; } 

        public MappedMethod(MappedComponent component, MethodInfo methodInfo, byte id) {
            Component = component;
            MethodInfo = methodInfo;
            Id = id;

            Invoke = CompileMethodInvoker(component.Type, methodInfo);
        }

        private static Func<object, object[], IRpcResponse> CompileMethodInvoker(Type type, MethodInfo method) {
            var targetObjectParameter = Expression.Parameter(typeof (object), "targetObject");
            var argumentArrayParameter = Expression.Parameter(typeof (object[]), "argumentArray");
            var arguments = method.GetParameters().Select((p, i) => ConvertMethodInfoParameter(p, i, argumentArrayParameter));
            var callExpression = 
                Expression.Call
                (
                    Expression.Convert
                    (
                        targetObjectParameter, 
                        type
                    ), 
                    method, 
                    arguments
                );

            if (method.ReturnType == typeof (void)) {
                var voidLambda = Expression.Lambda<Action<object, object[]>>(callExpression, targetObjectParameter, argumentArrayParameter);
                var compiledVoidlambda = voidLambda.Compile();

                return (target, args) => {
                    compiledVoidlambda(target, args);
                    return null;
                };
            }

            var lambda = Expression.Lambda<Func<Object, object[], IRpcResponse>>(callExpression, targetObjectParameter, argumentArrayParameter);
            return lambda.Compile();
        }

        private static Expression ConvertMethodInfoParameter(ParameterInfo parameterInfo, int index, ParameterExpression argumentArrayParameter) {
            return 
                Expression.Convert
                (
                    Expression.ArrayIndex
                    (
                        argumentArrayParameter, 
                        Expression.Constant(index)
                    ), 
                    parameterInfo.ParameterType
                );
        }
    }
}