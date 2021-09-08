using System;
using System.Collections.Generic;
using HexRuntimeAssemblier.Interfaces;
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

            IAssemblyBuilder builder = new AssemblyBuilder(new Dictionary<string, IAssemblyResolver>(), parser.start());
            builder.Build();
        }
    }
}
