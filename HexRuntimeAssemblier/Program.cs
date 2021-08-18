using System;
using Antlr4.Runtime;

namespace HexRuntimeAssemblier
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            var lexer = new AssemblierLexer(CharStreams.fromPath(@"..\test.il"));
#else
            var lexer = new AssemblierLexer(CharStreams.fromPath(args[1]));
#endif
            var parser = new Assemblier(new CommonTokenStream(lexer));
            var builder = new AssemblyBuilder();
            parser.AddParseListener(builder);

            parser.start();
        }
    }
}
