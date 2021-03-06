using System;
using System.Collections.Generic;

namespace HexRuntimeAssemblier.Meta
{
    public class DefinitionTable
    {
        private readonly MDRecordKinds mDefinitionKind;
        private readonly Dictionary<string, uint> mName2Token = new();
        private readonly List<object> mDefTokenMetas = new();
        public DefinitionTable(MDRecordKinds kind)
        {
            mDefinitionKind = kind;
        }
        /// <summary>
        /// Reference table kind
        /// </summary>
        public MDRecordKinds Kind => mDefinitionKind;
        /// <summary>
        /// Stores the reference token meta struture
        /// </summary>
        public IList<object> DefinitionTokenMetas => mDefTokenMetas;
        /// <summary>
        /// Indexer for meta object access
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public object this[uint token]
        {
            get => mDefTokenMetas[(int)token];
        }
        public uint GetDefinitionToken(string fullyQualifiedName, Func<object> metaGenerator)
        {
            if (!mName2Token.TryGetValue(fullyQualifiedName, out var token))
            {
                if (metaGenerator == null)
                    throw new SymbolNotFoundException($"Symbol '{fullyQualifiedName}' not found");
                token = (uint)mDefTokenMetas.Count;
                mDefTokenMetas.Add(metaGenerator());
                mName2Token.Add(fullyQualifiedName, token);
            }
            return token;
        }
    }
}
