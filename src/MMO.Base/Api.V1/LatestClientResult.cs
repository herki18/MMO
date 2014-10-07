using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMO.Base.Api.V1
{
    public class LatestClientResult
    {
        public BuildNumber Version { get; private set; }
        public string DownloadUrl { get; private set; }

        public LatestClientResult(BuildNumber version, string downloadUrl) {
            Version = version;
            DownloadUrl = downloadUrl;
        }
    }
}
