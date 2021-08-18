using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Tree;
using Antlr4.Runtime;

namespace HexRuntimeAssemblier
{
    public static class Helper
    {
        public static ITerminalNode GetToken(this ParserRuleContext context, int tokenType) => context.GetToken(tokenType, 0);
        public static T Get<T>(this ParserRuleContext context) where T : IParseTree => context.GetChild<T>(0);
    }
}
