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
        readonly AssemblyBuilder mResolver;
        readonly Dictionary<string, short> mLocalMap = new();
        readonly Dictionary<string, short> mArgumentMap = new();
        readonly Dictionary<string, int> mLabelMap = new();
        readonly List<(int Index, string LabelName)> mOffsetTable = new();
        public ILAssemblier(Assemblier.MethodBodyContext body, AssemblyBuilder assemblyResolver)
        {
            mAST = body;
            mResolver = assemblyResolver;
            mILWriter = new BinaryWriter(mILStream);
        }
        public ILMD Generate()
        {
            ILMD result = new();

            var locals = mAST.methodLocals().methodLocal();
            result.LocalVariables = locals.Select(x => new LocalVariableMD
            {
                NameToken = mResolver.MetaStringTable.GetTokenFromString(x.IDENTIFIER().GetText()),
                TypeRefToken = mResolver.ResolveType(x.type())
            }).ToArray();

            //Parse the IL code
            ParseIL(mAST.methodCode().ilInstruction());
            result.IL = mILStream.ToArray();

            return result;
        }
    }
}
