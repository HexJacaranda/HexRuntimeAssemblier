using Antlr4.Runtime;
using HexRuntimeAssemblier;
using HexRuntimeAssemblier.Interfaces;
using HexRuntimeAssemblier.Serialization;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace AssemblierTest
{
    public class SerializerTest
    {
        public static IAssemblyBuilder Build(string name,
            bool disableCoreType = true,
            bool coreLibrary = false)
        {
            var lexer = new AssemblierLexer(CharStreams.fromPath(@$"..\..\..\TestIL\{name}.il"));
            var parser = new Assemblier(new CommonTokenStream(lexer));

            IAssemblyBuilder builder = new AssemblyBuilder(
                CoreAssemblyConstant.Default,
                new AssemblyOptions()
                {
                    CoreLibrary = coreLibrary,
                    DisableCoreType = disableCoreType
                },
                new Dictionary<string, IAssemblyResolver>(),
                parser.start());

            builder.Build();
            return builder;
        }
        [Test]
        public void SerializeCoreLibToMem()
        {
            var builder = Build("TestCoreLib", false, true);
            var output = new MemoryStream();
            var serializer = new AssemblySerializer(output, builder);
            serializer.Serialize();
        }

        [Test]
        public void SerializeCoreLibToFile()
        {
            var builder = Build("TestCoreLib", false, true);
            using var output = File.OpenWrite(@"C:\HexRTLib\HexRT.Core");
            var serializer = new AssemblySerializer(output, builder);
            serializer.Serialize();
        }
    }
}
