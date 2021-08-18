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
        private uint mToken = 0u;
        private Dictionary<string, uint> mName2Token;
        public ReferenceTable(MDRecordKinds kind)
        {
            mReferenceKind = kind;
        }
        public MDRecordKinds Kind => mReferenceKind;
    }
}
