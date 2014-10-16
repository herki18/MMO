using System;
using System.ComponentModel;
using System.IO;
using System.Security.Policy;
using FluentAssertions;
using Microsoft.Win32.SafeHandles;
using MMO.Base.Infrastructure;
using NUnit.Framework;

namespace MMO.Tests.Base {
    [TestFixture]
    public class SimpleSerializerTests {
        public enum TestEnum : byte
        {
            One,
            Two = 6,
            Three = 10,
            Four = 25
        }

        public class Component1 : ICustomSerializer {
            public int Int1 { get; set; }
            public float Float1 { get; set; }
            public string[] StringArray { get; set; }

            public void Save(BinaryWriter writer, ISerializer serializer) {
                writer.Write(Int1);
                writer.Write(Float1);
                serializer.WriteObject(writer, typeof(string[]), StringArray);
            }

            public void Load(BinaryReader reader, ISerializer serializer) {
                Int1 = reader.ReadInt32();
                Float1 = reader.ReadSingle();
                StringArray = (string[]) serializer.ReadObject(reader, typeof (string[]));
            }
        }

        [Test]
        public void CanReadAndWriteGuid() {
            var guid = Guid.NewGuid();
            var ser = new SimpleSerializer();

            TestSerializer(
                bw => ser.WriteObject(bw, typeof(Guid), guid),
                br => ser.ReadObject(br, typeof(Guid)).Should().Be(guid));

        }

        [Test]
        public void CanReadAndWriteDatetimes() {
            var dateTime = new DateTime(1992, 1, 21);
            var ser = new SimpleSerializer();

            TestSerializer(
                bw => ser.WriteObject(bw, typeof(DateTime), dateTime),
                br => ser.ReadObject(br, typeof(DateTime)).Should().Be(dateTime));

        }

        [Test]
        public void CanReadAndWriteString()
        {
            const string testString = "Hello World!";

            var ser = new SimpleSerializer();

            TestSerializer(
                bw => ser.WriteObject(bw, typeof(string), testString),
                br => ser.ReadObject(br, typeof(string)).Should().Be(testString));
        }

        [Test]
        public void CanReadAndWriteArguments() {
            var types = new[] {typeof (bool), typeof (Guid), typeof (DateTime), typeof (ulong)};
            var arguments = new object[] {true, Guid.NewGuid(), new DateTime(1992, 2, 1), (ulong) 43399393939393};
            var ser = new SimpleSerializer();

            TestSerializer(
                bw => ser.WriteArguments(bw, types, arguments),
                br => {
                    var readArguments = ser.ReadArguments(br, types);
                    for (int i = 0; i < arguments.Length; i++) {
                        readArguments[i].Should().Be(arguments[i]);
                    }
                });

        }

        [Test]
        public void CanReadAndWriteArrayOfStrings() {
            var array = new[] {"Hello", "World", "Stuff", "Hey!"};

            var ser = new SimpleSerializer();

            TestSerializer(
                bw => ser.WriteObject(bw, typeof(string[]), array),
                br => {
                    var readArray = (string[])ser.ReadObject(br, typeof (string[]));
                    readArray.Length.Should().Be(array.Length);

                    for (int i = 0; i < array.Length; i++) {
                        array[i].Should().Be(readArray[i]);
                    }
                });
        }

        [Test]
        public void CanReadAndWriteArraysOfArraysOfString() {
            var array = new[] {new[] {"ONE", "TWO"}, new[] {"THREE", "FOUR"}, new[] {"FIVE", "SIX"}};

            var ser = new SimpleSerializer();

            TestSerializer(
                bw => ser.WriteObject(bw, typeof(string[][]), array),
                br => {
                    var readArray = (string[][]) ser.ReadObject(br, typeof (string[][]));
                    readArray.Length.Should().Be(array.Length);

                    for (int i = 0; i < readArray.Length; i++) {
                        var innerArray = readArray[i];
                        innerArray.Length.Should().Be(array[i].Length);

                        for (int j = 0; j < innerArray.Length; j++) {
                            innerArray[j].Should().Be(array[i][j]);
                        }
                    }
                });
        }

        [Test]
        [TestCase(TestEnum.One)]
        [TestCase(TestEnum.Two)]
        [TestCase(TestEnum.Three)]
        [TestCase(TestEnum.Four)]
        public void CanReadAndWriteEnumValue(TestEnum enumValue) {
            var ser = new SimpleSerializer();

            TestSerializer(
                bw => ser.WriteObject(bw, typeof(TestEnum), enumValue),
                br => ser.ReadObject(br, typeof(TestEnum)).Should().Be(enumValue));
        }

        [Test]
        public void CanReadAndWriteEnumValuesToArray() {
            var array = new[] {TestEnum.One, TestEnum.Two, TestEnum.Three, TestEnum.Four};

            var ser = new SimpleSerializer();

            TestSerializer(
                bw => ser.WriteObject(bw, typeof(TestEnum[]), array),
                br => {
                    var readArray = (TestEnum[]) ser.ReadObject(br, typeof (TestEnum[]));
                    readArray.Length.Should().Be(array.Length);

                    for (int i = 0; i < readArray.Length; i++) {
                        readArray[i].Should().Be(array[i]);
                    }
                });
        }

        [Test]
        public void CanReadAndWriteNullableValueTypes() {
            var ser = new SimpleSerializer();
            var int1 = 123;
            int? int2 = null;
            int? int3 = 456;

            TestSerializer(
                bw => {
                    ser.WriteObject(bw, typeof(int), int1);
                    ser.WriteObject(bw, typeof(int?), int2);
                    ser.WriteObject(bw, typeof(int?), int3);
                },
                br => {
                    ser.ReadObject(br, typeof (int)).Should().Be(int1);
                    ser.ReadObject(br, typeof (int?)).Should().Be(int2);
                    ser.ReadObject(br, typeof (int?)).Should().Be(int3);
                });

        }

        [Test]
        public void CanReadAndWriteNullableString() {
            var ser = new SimpleSerializer();
            var str1 = "hey";
            string str2 = null;

            TestSerializer(
                bw => {
                    ser.WriteObject(bw, typeof(string), str1);
                    ser.WriteObject(bw, typeof(string), str2);
                },
                br => {
                    ser.ReadObject(br, typeof (string)).Should().Be(str1);
                    ser.ReadObject(br, typeof (string)).Should().Be(str2);
                });

        }

        [Test]
        public void CanReadAndWriteArraysWithNullValues() {
            var ser = new SimpleSerializer();

            var arr = new[] {"Hey", null, "Stugg"};

            TestSerializer(
                bw => ser.WriteObject(bw, typeof(string[]), arr),
                br => {
                    var readArr = (string[]) ser.ReadObject(br, typeof (string[]));
                    readArr.Length.Should().Be(arr.Length);

                    for (int i = 0; i < readArr.Length; i++) {
                        readArr[i].Should().Be(arr[i]);
                    }
                });
        }

        [Test]
        public void CanReadAndWriteCustomObject() {
            var ser = new SimpleSerializer();

            var component1 = new Component1() {
                Float1 = 3.14f,
                Int1 = 256,
                StringArray = new[] {  "Hello", "World", null, "Blegh"}
            };

            TestSerializer(
            bw => ser.WriteObject(bw, typeof(Component1), component1),
                br => {
                    var readComponent = (Component1) ser.ReadObject(br, typeof (Component1));
                    readComponent.Should().NotBeNull();
                    readComponent.Int1.Should().Be(component1.Int1);
                    readComponent.Float1.Should().Be(component1.Float1);

                    readComponent.StringArray.Length.Should().Be(component1.StringArray.Length);
                    for (int i = 0; i < readComponent.StringArray.Length; i++) {
                        readComponent.StringArray[i].Should().Be(component1.StringArray[i]);
                    }
                });
        }

        private void TestSerializer(Action<BinaryWriter> write, Action<BinaryReader> read) {
            byte[] bytes;

            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                write(bw);
                bytes = ms.ToArray();
            }

            using (var ms = new MemoryStream(bytes))
            using (var br = new BinaryReader(ms))
            {
                read(br);
            }

        }

        
    }
}