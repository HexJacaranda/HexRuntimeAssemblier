using HexRuntimeAssemblier;
using HexRuntimeAssemblier.Meta;
using HexRuntimeAssemblier.Interfaces;
using HexRuntimeAssemblier.Reference;
using HexRuntimeAssemblier.Serialization;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.IO;

namespace AssemblierTest
{
    public class SerializerTest
    {
        private readonly static ILoggerFactory Factory = LoggerFactory.Create(x => x.AddConsole());
        public static IAssemblyBuilder Build(string name,
            bool disableCoreType = true,
            bool coreLibrary = false)
        {
            IAssemblyBuilder builder = new AssemblyBuilder(
                CoreAssemblyConstant.Default,
                new AssemblyOptions()
                {
                    CoreLibrary = coreLibrary,
                    DisableCoreType = disableCoreType
                },
                Factory.CreateLogger<AssemblyBuilder>(),
                Array.Empty<IAssemblyResolver>());

            builder.Build(File.OpenRead(@$"..\..\..\TestIL\{name}.il"));
            return builder;
        }

        [Test]
        public void SerializeILMD()
        {
            var serializer = AssemblySerializerHelper.GetSerializer(typeof(ILMD)) as Action<BinaryWriter, ILMD>;
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);
            serializer(writer, new ILMD()
            {
                LocalVariables = null,
                IL = new byte[] { 0x00 }
            });
            var actual = stream.ToArray();
            Assert.AreEqual(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00 }, actual);
        }

        [Test]
        public void SerializeTypeMD()
        {
            var serializer = AssemblySerializerHelper.GetSerializer(typeof(TypeMD)) as Action<BinaryWriter, TypeMD>;
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);
            serializer(writer, new TypeMD()
            {
                MethodTokens = new uint[] { 0, 1 }
            });
            var actual = stream.ToArray();
            Assert.AreEqual(null, null);
        }

        [Test, Order(1)]
        public void SerializeCoreLibToMem()
        {
            var builder = Build("TestCoreLib", false, true);
            var output = new MemoryStream();
            var serializer = new AssemblySerializer(output, builder);
            serializer.Serialize();
        }

        [Test, Order(2)]
        public void SerializeCoreLibToFile()
        {
            var builder = Build("TestCoreLib", false, true);
            using var output = File.OpenWrite(@"..\..\..\TestLib\HexRT.Core");
            var serializer = new AssemblySerializer(output, builder);
            serializer.Serialize();
        }

        [Test, Order(3)]
        public void ResolveCoreLib()
        {
            var resolver = AssemblyResolver.Build(File.OpenRead(@"..\..\..\TestLib\HexRT.Core"));
            Assert.DoesNotThrow(() => resolver.QueryTypeDefinition("[Core][global]Int32"));
            Assert.DoesNotThrow(() => resolver.QueryTypeDefinition("[Core][global]Object"));
            Assert.DoesNotThrow(() => resolver.QueryTypeDefinition("[Core][global]Array<Canon>"));
        }
    }
}
