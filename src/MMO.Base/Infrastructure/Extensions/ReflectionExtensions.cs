using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MMO.Base.Infrastructure.Extensions {
    public static class ReflectionExtensions {
        public static MethodInfo GetMethodBySignature(this Type that, string signature) {
            var parts = signature.Split('%');
            var parameterTypes = parts.Skip(1).Where(t => !string.IsNullOrEmpty(t)).Select(Type.GetType);
            return that.GetMethod(parts[0], parameterTypes.ToArray());
        }

        public static string GetMethodSignature(this MethodInfo that) {
            var builder = new StringBuilder();
            builder.AppendFormat("{0}%", that.Name);
            builder.Append(string.Join("%", that.GetParameters().Select(t => t.ParameterType.AssemblyQualifiedName).ToArray()));
            return builder.ToString();
        }
    }
}