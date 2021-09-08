using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HexRuntimeAssemblier.Interfaces;

namespace HexRuntimeAssemblier.Reference
{
    public class AssemblyResolver : IAssemblyResolver
    {
        public uint QueryFieldDefinition(string fullQualifiedName)
        {
            return 0u;
        }

        public uint QueryMethodDefinition(string fullQualifiedName)
        {
            return 0u;
        }

        public uint QueryTypeDefinition(string fullQualifiedName)
        {
            return 0u;
        }
    }
}
