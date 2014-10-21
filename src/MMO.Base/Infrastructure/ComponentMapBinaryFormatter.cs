using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Schema;
using MMO.Base.Infrastructure.Extensions;

namespace MMO.Base.Infrastructure
{
    public class ComponentMapBinaryFormatter
    {
        public void Load(BinaryReader reader, ComponentMap map) {
            map.SetReserverComponentIdLimit(reader.ReadByte());
            var componentCount = reader.ReadInt32();

            for (int i = 0; i < componentCount; i++) {
                var componentId = reader.ReadByte();
                var componentType = Type.GetType(reader.ReadString());
                var component = map.MapComponent(componentType, componentId);
                var methodCount = reader.ReadInt32();

                for (int j = 0; j < methodCount; j++) {
                    var methodId = reader.ReadByte();
                    var methodInfo = componentType.GetMethodBySignature(reader.ReadString());
                    component.MapMethod(methodInfo, methodId);
                }

            }
        }

        public ComponentMap Load(BinaryReader reader) {
            var map = new ComponentMap();
            Load(reader, map);
            return map;
        }

        public void Save(BinaryWriter writer, ComponentMap map) {
            writer.Write(map.ReservedComponentIdLimit);
            writer.Write(map.ComponentCount);

            foreach (var component in map.AllComponents) {
                writer.Write(component.Id);
                writer.Write(component.Type.AssemblyQualifiedName);

                writer.Write(component.MethodCount);

                foreach (var method in component.AllMethods) {
                    writer.Write(method.Id);
                    writer.Write(method.MethodInfo.GetMethodSignature());
                }
            }

        }
    }
}
