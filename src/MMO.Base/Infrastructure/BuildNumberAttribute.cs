using System;
using System.Linq;

namespace MMO.Base.Infrastructure
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class BuildNumberAttribute : Attribute
    {
        public static BuildNumberAttribute CurrentBuildNumber() {
            var attribute = AppDomain.CurrentDomain.GetAssemblies().Select(
                assembly =>
                    assembly.GetCustomAttributes(typeof (BuildNumberAttribute), false).OfType<BuildNumberAttribute>().FirstOrDefault());

            return attribute.FirstOrDefault(t => t != null);
        }

        public BuildNumber Number { get; set; }
        public bool IsDebug { get; set; }

        public BuildNumberAttribute(short buildVersion, int timestamp, bool isDebug) {
            Number = new BuildNumber(buildVersion, timestamp);
            IsDebug = isDebug;
        }
    }
}
