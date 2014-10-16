using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MMO.Base.Infrastructure {
    public class SimpleSerializer : ISerializer {

        

        public void WriteArguments(BinaryWriter writer, Type[] types, object[] arguments) {
            for (int i = 0; i < types.Length; i++) {
                WriteObject(writer, types[i], arguments[i]);
            }
        }

        public object[] ReadArguments(BinaryReader reader, Type[] types) {
            var arguments = new object[types.Length];
            
            for (int i = 0; i < types.Length; i++) {
                arguments[i] = ReadObject(reader, types[i]);
            }

            return arguments;
        }

        public void WriteObject(BinaryWriter writer, Type type, object value) {
            if (value == null) {
                writer.Write(false);
                return;
            }

            var isNullableValueType = type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>);
            if (!type.IsValueType || isNullableValueType) {
                writer.Write(true);
            }

            if (isNullableValueType) {
                WriteSimple(writer, type.GetGenericArguments()[0], value);
            }
            else if (type.IsArray) {
                var array = (Array) value;
                var elementType = type.GetElementType();

                writer.Write(array.Length);
                foreach (var item in array) {
                    WriteObject(writer, elementType, item);
                }
            } else if (type.IsEnum) {
                WriteSimple(writer, Enum.GetUnderlyingType(type), value);
            }else if (IsCustomSerializer(type)) {
                ((ICustomSerializer)value).Save(writer, this);
            }
            else {
                WriteSimple(writer, type, value);
            }
        }

        public object ReadObject(BinaryReader reader, Type type) {
            if (!type.IsValueType) {
                if (reader.ReadBoolean() == false) {
                    return null;
                }
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>)) {
                if (reader.ReadBoolean() == false) {
                    return null;
                }

                return ReadSimple(reader, type.GetGenericArguments()[0]);
            }


            if (type.IsArray) {
                var elementType = type.GetElementType();
                var length = reader.ReadInt32();
                var array = Array.CreateInstance(elementType, length);

                for (int i = 0; i < length; i++) {
                    array.SetValue(ReadObject(reader, elementType), i);
                }

                return array;
            }

            if (type.IsEnum) {
                return Enum.ToObject(type,ReadSimple(reader, Enum.GetUnderlyingType(type)));
            }

            if (IsCustomSerializer(type)) {
                var value = (ICustomSerializer) Activator.CreateInstance(type);
                value.Load(reader, this);
                return value;
            }

            return ReadSimple(reader, type);
        }

        private void WriteSimple(BinaryWriter writer, Type type, object value)
        {
            if (type == typeof (uint)) {
                writer.Write((uint)value);
            }
            else if (type == typeof (byte)) {
                writer.Write((byte)value);
            }
            else if (type == typeof (string)) {
                writer.Write((string)value);
            }
            else if (type == typeof (float)) {
                writer.Write((float)value);
            }
            else if (type == typeof (double)) {
                writer.Write((double)value);
            }
            else if (type == typeof (decimal)) {
                writer.Write((decimal)value);
            }
            else if (type == typeof (int)) {
                writer.Write((int)value);
            }
            else if (type == typeof (short)) {
                writer.Write((short)value);
            }
            else if (type == typeof (ushort)) {
                writer.Write((ushort)value);
            }
            else if (type == typeof (long)) {
                writer.Write((long)value);
            }
            else if (type == typeof (ulong)) {
                writer.Write((ulong)value);
            }
            else if (type == typeof (char)) {
                writer.Write((char)value);
            }
            else if (type == typeof (bool)) {
                writer.Write((bool) value);
            }
            else if(type == typeof(Guid)) {
                writer.Write(((Guid)value).ToByteArray());
            }
            else if (type == typeof (DateTime)) {
                writer.Write(((DateTime)value).Ticks);
            }
            else {
                throw new ArgumentException(string.Format("Cannot write '{0}'", type.FullName), "value");
            }
        }

        private object ReadSimple(BinaryReader reader, Type type) {
            if (type == typeof(uint)){
                return reader.ReadUInt32();
            }
            if (type == typeof(byte)) {
                return reader.ReadByte();
            }
            if (type == typeof(string)) {
                return reader.ReadString();
            }
            if (type == typeof(float)) {
                return reader.ReadSingle();
            }
            if (type == typeof(double)){
                return reader.ReadDouble();
            }
            if (type == typeof(decimal)){
                return reader.ReadDecimal();
            }
            if (type == typeof(int)){
                return reader.ReadInt32();
            }
            if (type == typeof(short)){
                return reader.ReadInt16();
            }
            if (type == typeof(ushort)){
                return reader.ReadUInt16();
            }
            if (type == typeof(long)){
                return reader.ReadInt64();
            }
            if (type == typeof(ulong)){
                return reader.ReadUInt64();
            }
            if (type == typeof(char)){
                return reader.ReadChar();
            }
            if (type == typeof (bool)) {
                return reader.ReadBoolean();
            }
            if (type == typeof(Guid)){
                return new Guid(reader.ReadBytes(16));
            }
            if (type == typeof(DateTime)){
                return new DateTime(reader.ReadInt64());
            }

            throw new ArgumentException(string.Format("Cannot read '{0}'", type.FullName), "value");
        }

        private static bool IsCustomSerializer(Type type) {
            return typeof (ICustomSerializer).IsAssignableFrom(type);
        }
    }
}