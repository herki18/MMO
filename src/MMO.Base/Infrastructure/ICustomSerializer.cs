using System.IO;

namespace MMO.Base.Infrastructure {
    public interface ICustomSerializer {
        void Save(BinaryWriter writer, ISerializer serializer);
        void Load(BinaryReader reader, ISerializer serializer);
    }
}