using NUnit.Framework;
using HexRuntimeAssemblier;
using Antlr4.Runtime;

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

            var builder = new AssemblyBuilder();
            var start = parser.start();
            builder.ResolveStart(start);
        }
    }
}