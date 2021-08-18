using System;
using System.Collections.Generic;
using System.Linq;

namespace HexRuntimeAssemblier
{
    public class AssemblyBuilder
    {
        private AssemblyHeaderMD mCurrentAssembly;
        private ReferenceResolver mReferenceResolver;
        private uint mTypeDefToken = 0u;
        private Dictionary<uint, TypeMD> mTypeMapping;
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
        public void ResolveStart(Assemblier.StartContext context)
        {
            ResolveAssemblyDef(context.GetChild<Assemblier.AssemblyDefContext>(0));
            foreach (var classContext in context.children.OfType<Assemblier.ClassDefContext>())
                ResolveClassDef(classContext);
        }
        public void ResolveAssemblyDef(Assemblier.AssemblyDefContext context)
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

        public uint ResolveClassRef(Assemblier.TypeRefContext context)
        {
            var assemblyRef = context.Get<Assemblier.AssemblyRefContext>();
            if (assemblyRef == null)
            {
                string typeName = context.GetText();
                if (!mTypeName2Token.TryGetValue(typeName, out var token))
                {
                    token = mTypeDefToken++;
                    mTypeName2Token.Add(typeName, token);
                }
                return mReferenceResolver.AcquireInternalTypeReference(typeName, token);
            }
            else
            {
                return mReferenceResolver.ResolveTypeReference(assemblyRef.GetText(), context.GetText());
            }
        }

        public void ResolveClassDef(Assemblier.ClassDefContext context)
        {
            var name = context.Get<Assemblier.TypeNameContext>();

            TypeFlag flag = 0;
            if (context.GetToken(Assemblier.MODIFIER_NEST) != null)
                flag |= TypeFlag.Nested;
            if (context.GetToken(Assemblier.MODIFIER_SEALED) != null)
                flag |= TypeFlag.Sealed;
            if (context.GetToken(Assemblier.MODIFIER_ABSTRACT) != null)
                flag |= TypeFlag.Abstract;
            if (context.GetToken(Assemblier.MODIFIER_INTERFACE) != null)
                flag |= TypeFlag.Interface;

            var type = new TypeMD();
            var parent = context.Get<Assemblier.TypeInheritContext>();
            if (parent != null)
            {
                var parentTypeRef = parent.Get<Assemblier.TypeRefContext>();
                type.ParentTypeRefToken = ResolveClassRef(parentTypeRef);
            }


        }
    }
}
