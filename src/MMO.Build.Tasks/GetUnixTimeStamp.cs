

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using MMO.Base;
using MMO.Base.Infrastructure;

namespace MMO.Build.Tasks
{
    public class GetUnixTimestamp : Task
    {
        [Output]
        public string TimeStamp { get; set; }

        public override bool Execute() {
            TimeStamp = Time.GetUnixTimeStamp().ToString();
            return true;
        }
    }
}
