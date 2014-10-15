using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using FluentAssertions;
using MMO.Base.Infrastructure;
using NUnit.Framework;

namespace MMO.Tests.Base {
    [TestFixture]
    public class ComponentMapBinaryFormatterTests {
        interface ITestComponent {
            void Method1();
            void Method2();
        }

        interface ITestComponent2 {
            void OverloadedMethod1();
            void OverloadedMethod1(bool whoa);
        }

        [Test]
        public void CanLoadAndSaveComponentMap() {
            var testComponentType = typeof (ITestComponent);
            var testComponentMethod1 = testComponentType.GetMethod("Method1");
            var testComponentMethod2 = testComponentType.GetMethod("Method2");

            var testComponentType2 = typeof(ITestComponent2);
            var testComponentOverloadMethod1 = testComponentType2.GetMethod("OverloadedMethod1", new Type[0]);
            var testComponentOverloadMethod2 = testComponentType2.GetMethod("OverloadedMethod1", new[] { typeof(bool)});


            var map = new ComponentMap(3);
            var testComponentMap = map.MapComponent(testComponentType, 3);
            testComponentMap.MapMethod(testComponentMethod1, 2);
            testComponentMap.MapMethod(testComponentMethod2, 10);

            var testComponent2Map = map.MapComponent(testComponentType2, 10);
            testComponent2Map.MapMethod(testComponentOverloadMethod1, 43);
            testComponent2Map.MapMethod(testComponentOverloadMethod2, 12);

            var binaryFormatter = new ComponentMapBinaryFormatter();
            byte[] savedBytes;
            using (var ms = new MemoryStream()) {
                using (var bw = new BinaryWriter(ms)) {
                    binaryFormatter.Save(bw, map);
                    savedBytes = ms.ToArray();
                }
            }

            using (var ms = new MemoryStream(savedBytes)) {
                ms.Seek(0, SeekOrigin.Begin);

                using (var br = new BinaryReader(ms)) {
                    var map2 = binaryFormatter.Load(br);

                    map2.ReservedComponentIdLimit.Should().Be(3);
                    map2.Components[3].Type.Should().Be(testComponentType);
                    map2.Methods[3][2].MethodInfo.Should().BeSameAs(testComponentMethod1);
                    map2.Methods[3][10].MethodInfo.Should().BeSameAs(testComponentMethod2);

                    map2.Components[10].Type.Should().Be(testComponentType2);
                    map2.Methods[10][43].MethodInfo.Should().BeSameAs(testComponentOverloadMethod1);
                    map2.Methods[10][12].MethodInfo.Should().BeSameAs(testComponentOverloadMethod2);

                }
            }
        }
    }
}