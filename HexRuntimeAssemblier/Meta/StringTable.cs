using System.Collections.Generic;

using MDToken = System.UInt32;
namespace HexRuntimeAssemblier.Meta
{
    public class StringTable
    {
        private readonly Dictionary<string, MDToken> mStringPool = new();
        private readonly List<string> mStrings = new();
        public MDRecordKinds Kind => MDRecordKinds.String;
        public IReadOnlyList<string> Contents => mStrings;
        public MDToken GetTokenFromString(string value)
        {
            if (!mStringPool.TryGetValue(value, out var token))
            {
                token = (MDToken)mStrings.Count;
                mStringPool.Add(value, token);
                mStrings.Add(value);
            }
            return token;
        }
    }
}
