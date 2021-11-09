using System.IO;
using System.Collections.Generic;
using System.Linq;
using HexRuntimeAssemblier.Interfaces;
using HexRuntimeAssemblier.Meta;
using HexRuntimeAssemblier.Reference;

namespace HexRuntimeAssemblier.IL
{

    /// <summary>
    /// Use il tree to generate byte stream of IL code
    /// </summary>
    partial class ILAssemblier
    {
        readonly MemoryStream mILStream = new(16);
        readonly BinaryWriter mILWriter;
        readonly Assemblier.MethodBodyContext mAST;
        readonly Assemblier.MethodArgumentListContext mArguments;
        readonly AssemblyBuilder mResolver;
        readonly Dictionary<string, short> mLocalMap = new();
        readonly Dictionary<string, short> mArgumentMap = new();
        readonly Dictionary<string, int> mLabelMap = new();
        readonly List<(int Index, string LabelName)> mOffsetTable = new();
        public ILAssemblier(Assemblier.MethodBodyContext body, Assemblier.MethodArgumentListContext arguments, AssemblyBuilder assemblyResolver)
        {
            mAST = body;
            mArguments = arguments;
            mResolver = assemblyResolver;
            mILWriter = new BinaryWriter(mILStream);
        }
        public ILMD Generate()
        {
            ILMD result = new();

            var locals = mAST.methodLocals()?.methodLocal();
            if (locals != null)
            {
                result.LocalVariables = locals.Select(x => new LocalVariableMD
                {
                    NameToken = mResolver.MetaStringTable.GetTokenFromString(x.IDENTIFIER().GetText()),
                    TypeRefToken = mResolver.ResolveType(x.type())
                }).ToArray();
            }

            //Set local mapping
            if (locals != null)
            {
                short localIndex = 0;
                foreach (var local in locals)
                    mLocalMap[local.IDENTIFIER().GetText()] = localIndex++;
            }

            //Set argument mapping
            if(mArguments != null)
            {
                short argumentIndex = 0;
                foreach (var argument in mArguments.methodArgument())
                    mArgumentMap[argument.IDENTIFIER().GetText()] = argumentIndex++;
            }

            //Parse the IL code
            ParseIL(mAST.methodCode().ilInstruction());
            result.IL = mILStream.ToArray();

            return result;
        }
    }
}
