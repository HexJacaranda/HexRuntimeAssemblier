using HexRuntimeAssemblier;
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
