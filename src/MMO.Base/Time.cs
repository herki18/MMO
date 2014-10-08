using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMO.Base
{
    public static class Time
    {
        public static int GetUnixTimeStamp() {
            return GetUnixTimeStamp(DateTime.UtcNow);
        }

        public static int GetUnixTimeStamp(DateTime time) {
            var timeStamp = (time.ToUniversalTime() - new DateTime(1970, 1,1,0,0,0));
            return (int) timeStamp.TotalSeconds;
        }
    }
}
