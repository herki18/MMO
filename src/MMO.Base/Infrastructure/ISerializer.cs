using System;
using System.Collections.Generic;
using System.IO;

namespace MMO.Base.Infrastructure {
    public interface ISerializer {
        void WriteArguments(BinaryWriter writer, Type[] types, object[] arguments);
        object[] ReadArguments(BinaryReader reader, Type[] types);

        void WriteObject(BinaryWriter writer, Type type, object value);
        object ReadObject(BinaryReader reader, Type type);
    }
}