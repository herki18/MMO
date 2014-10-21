using System;
using Castle.DynamicProxy;

namespace MMO.Base.Infrastructure {
    public class ComponentProxyGenerator {
        private static ProxyGenerator _proxyGenerator;

        public static object CreateInterfaceProxyWithoutTarget(Type interfaceType, params IInterceptor[] interceptors) {
            if (_proxyGenerator == null) {
                _proxyGenerator = new ProxyGenerator();
            }

            return _proxyGenerator.CreateInterfaceProxyWithoutTarget(interfaceType, interceptors);
        }
    }
}