using System;
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