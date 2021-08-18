using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexRuntimeAssemblier
{
    class AssemblyBuilder: AssemblierBaseListener
    {
        private AssemblyHeaderMD mCurrentAssembly;
        private uint mClassDefToken = 0u;
        private Dictionary<uint, Type> mTypeMapping;
        private Dictionary<string, uint> mTypeName2Token;

        private Dictionary<string, uint> mString2Token = new();
        private uint mStringToken = 0u;

        public static string GetPropertyKey(Assemblier.PropertyContext context)
            => context.GetChild<Assemblier.PropertyKeyContext>(0).GetText();
        public static string GetPropertyValue(Assemblier.PropertyContext context)
            => context.GetChild<Assemblier.PropertyValueContext>(0).GetText();
        public uint GetTokenFromString(string value)
        {
            if (!mString2Token.TryGetValue(value, out var token))
            {
                token = mStringToken++;
                mString2Token.Add(value, token);
            }
            return token;
        }
        public override void EnterAssemblyDef(Assemblier.AssemblyDefContext context)
        {
            var properties = context.children.OfType<Assemblier.PropertyContext>();
            var map = properties.ToDictionary(x => GetPropertyKey(x), x => GetPropertyValue(x));

            mCurrentAssembly = new AssemblyHeaderMD
            {
                NameToken = GetTokenFromString(map["name"]),
                GroupNameToken = GetTokenFromString(map["groupname"]),
                MajorVersion = int.Parse(map["major"]),
                MinorVersion = int.Parse(map["minor"]),
                GUID = Guid.Parse(map["guid"])
            };
        }

        public override void EnterClassDef(Assemblier.ClassDefContext context)
        {
            
        }
    }
}
