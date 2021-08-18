using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexRuntimeAssemblier
{
    class ExternalAssembly
    {
        public uint AssemblyReferenceToken { get; }
        public uint GetTypeDefToken(string referenceName)
        {

        }
        public uint GetMemberDefToken(string referenceName, MDRecordKinds kind)
        {

        }
    }
}
