using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMO.Base
{
    public class BuildNumber : IEquatable<BuildNumber> {
        public short Version { get; private set; }
        public int Timestamp { get; private set; }
        
        protected BuildNumber() { }

        public BuildNumber(short version, int timestamp) {
            Version = version;
            Timestamp = timestamp;
        }

        public override string ToString() {
            return string.Format("{0}.{1}", Version, Timestamp);
        }

        public bool Equals(BuildNumber other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Version == other.Version && Timestamp == other.Timestamp;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BuildNumber) obj);
        }

        public override int GetHashCode() {
            unchecked {
                return (Version.GetHashCode()*397) ^ Timestamp;
            }
        }

        public static bool operator ==(BuildNumber left, BuildNumber right) {
            return Equals(left, right);
        }

        public static bool operator !=(BuildNumber left, BuildNumber right) {
            return !Equals(left, right);
        }
    }
}
