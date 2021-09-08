using Antlr4.Runtime;
using HexRuntimeAssemblier;
using HexRuntimeAssemblier.Interfaces;
using NUnit.Framework;
using System.Collections.Generic;

namespace AssemblierTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var lexer = new AssemblierLexer(CharStreams.fromPath(@"..\..\..\TestIL\test.il"));
            var parser = new Assemblier(new CommonTokenStream(lexer));

            IAssemblyBuilder builder = new AssemblyBuilder(new Dictionary<string, IAssemblyResolver>(), parser.start());
            builder.Build();
        }
    }
}