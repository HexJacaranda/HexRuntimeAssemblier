using System;
using System.Collections.Generic;

namespace HexRuntimeAssemblier.Meta
{
    public class ReferenceTable
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
        public T GetMeta<T>(uint token) where T : class => mRefTokenMetas[(int)token] as T;
        public uint GetReferenceToken(string fullQualifiedName, Func<object> metaGenerator)
        {
            if (!mName2Token.TryGetValue(fullQualifiedName, out var token))
            {
                token = (uint)mRefTokenMetas.Count;
                mRefTokenMetas.Add(metaGenerator());
                mName2Token.Add(fullQualifiedName, token);
            }
            return token;
        }
    }
}
