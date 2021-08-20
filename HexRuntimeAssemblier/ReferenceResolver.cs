using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexRuntimeAssemblier
{
    /// <summary>
    /// To resolve the external reference: type, method and field etc, from the
    /// external library.
    /// </summary>
    class ReferenceResolver
    {
        private readonly Dictionary<string, ExternalAssembly> mExternalAssemblies = new();
        private readonly Dictionary<MDRecordKinds, ReferenceTable> mReferenceTables = new();
        public IEnumerable<ReferenceTable> ReferenceTables => mReferenceTables.Values;
        /// <summary>
        /// Resolve the external type, and get the reference token
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="typeReference"></param>
        /// <returns></returns>
        public uint ResolveTypeReference(string assembly, string typeReference)
        {
            if (!mExternalAssemblies.TryGetValue(assembly, out var externalAssembly))
                throw new ReferenceTargetNotFoundException($"Reference assembly [{assembly}] not found");
            var typeDefToken = externalAssembly.GetTypeDefToken(typeReference);
            var typeRefTable = mReferenceTables[MDRecordKinds.TypeRef];
            
            return typeRefTable.GetReferenceToken(typeReference, 
                () => new TypeRefMD { AssemblyToken = externalAssembly.AssemblyReferenceToken, Token = typeDefToken });
        }
        /// <summary>
        /// Get internal type reference token
        /// </summary>
        /// <param name="typeReference"></param>
        /// <param name="typeDefToken"></param>
        /// <returns></returns>
        public uint AcquireInternalTypeReference(string typeReference, uint typeDefToken)
        {
            var typeRefTable = mReferenceTables[MDRecordKinds.TypeRef];
            return typeRefTable.GetReferenceToken(typeReference,
                () => new TypeRefMD { AssemblyToken = AssemblyRefMD.Self, Token = typeDefToken });
        }
        public uint ResovleMethodReference(string assembly, string typeReference, string methodReference)
        {
            return 0u;
        }
    }
}
