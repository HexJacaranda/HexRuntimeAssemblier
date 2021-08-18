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
        public uint ResolveTypeReference(string assembly, string typeReference)
        {
            return 0u;
        }

        public uint ResovleMethodReference(string assembly, string methodReference)
        {
            return 0u;
        }

        public uint ResovleFieldReference(string assembly, string fieldReference)
        {
            return 0u;
        }
    }
}
