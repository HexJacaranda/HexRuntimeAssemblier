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
        public uint QueryFieldDefinition(string fullyQualifiedName)
        {
            return 0u;
        }

        public uint QueryMethodDefinition(string fullyQualifiedName)
        {
            return 0u;
        }

        public uint QueryTypeDefinition(string fullyQualifiedName)
        {
            return 0u;
        }
    }
}
