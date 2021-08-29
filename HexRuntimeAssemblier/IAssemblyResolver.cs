using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexRuntimeAssemblier
{
    public interface IAssemblyResolver
    {
        uint GetTokenFromString(string value);
        uint ResolveTypeRef(Assemblier.TypeRefContext context);
        uint ResolveMethodRef(Assemblier.MethodRefContext context);
        public uint ResolveFieldRef(Assemblier.FieldRefContext context);
    }
}
