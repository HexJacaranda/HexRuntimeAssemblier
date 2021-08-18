using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexRuntimeAssemblier
{
    class ReferenceTable
    {
        private readonly MDRecordKinds mReferenceKind;
        private readonly Dictionary<string, uint> mName2Token = new();
        private readonly List<object> mRefTokenMetas = new();
        public ReferenceTable(MDRecordKinds kind)
        {
            mReferenceKind = kind;
        }
        /// <summary>
        /// Reference table kind
        /// </summary>
        public MDRecordKinds Kind => mReferenceKind;
        /// <summary>
        /// Stores the reference token meta struture
        /// </summary>
        public IList<object> ReferenceTokenMetas => mRefTokenMetas;
        public uint GetReferenceToken(string fullQualifiedName, Func<object> metaGenerator)
        {
            if (!mName2Token.TryGetValue(fullQualifiedName, out var token))
            {
                token = (uint)mRefTokenMetas.Count;
                mRefTokenMetas.Add(metaGenerator());
            }
            return token;
        }
    }
}
