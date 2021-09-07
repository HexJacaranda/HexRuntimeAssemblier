using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HexRuntimeAssemblier.Interfaces;

namespace HexRuntimeAssemblier.Reference
{
    //R
    public class AssemblyResolver : IAssemblyResolver
    {
        public uint QueryFieldReference(string fullQualifiedName)
        {
            throw new NotImplementedException();
        }

        public uint QueryMethodReference(string fullQualifiedName)
        {
            throw new NotImplementedException();
        }

        public uint QueryTypeReference(string fullQualifiedName)
        {
            throw new NotImplementedException();
        }
    }
}
