using System.IO;
using System.Linq;

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
        readonly IAssemblyResolver mResolver;
        public ILAssemblier(Assemblier.MethodBodyContext body, IAssemblyResolver assemblyResolver)
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
                NameToken = mResolver.GetTokenFromString(x.IDENTIFIER().GetText()),
                TypeRefToken = mResolver.ResolveTypeRef(x.typeRef())
            }).ToArray();

            //Parse the IL code
            ParseIL(mAST.methodCode().ilInstruction());
            result.IL = mILStream.ToArray();

            return result;
        }
    }
}
