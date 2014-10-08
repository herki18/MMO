using System.IO;
using Microsoft.Build.Utilities;

namespace MMO.Build.Tasks
{
    public class UpdateBuildInfo : Task
    {
        public string Timestamp { get; set; }
        public string VersionNumber { get; set; }
        public string Env { get; set; }
        public string Files { get; set; }

        public override bool Execute() {
            var isDebug = Env == "dev" ? "true" : "false";
            var fileContents = string.Format("[assembly: MMO.Base.BuildNumber({0}, {1}, {2})]", VersionNumber, Timestamp, isDebug);

            foreach (var file in Files.Split(',')) {
                File.WriteAllText(file, fileContents);
            }

            return true;
        }
    }
}
