using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System.Linq;
using System.Collections.Generic;

namespace HexRuntimeAssemblier
{
    public static class Helper
    {
        public static bool ExistToken(this ParserRuleContext context, int tokenType) 
            => context.GetToken(tokenType, 0) != null;
        public static IEnumerable<T> OfType<T>(this ParserRuleContext context) 
            => context.children.OfType<T>();
        public static int GetUnderlyingTokenType(this ParserRuleContext context)
            => (context.children[0] as ITerminalNode).Symbol.Type;
        public static ParserRuleContext GetUnderlyingType(this ParserRuleContext context) 
            => context.children.First(x => x is ParserRuleContext) as ParserRuleContext;
    }
}
