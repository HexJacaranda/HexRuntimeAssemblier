using HexRuntimeAssemblier.Interfaces;
using HexRuntimeAssemblier.Meta;
using System.Collections.Generic;
using System;
using MDToken = System.UInt32;

namespace HexRuntimeAssemblier
{
    public partial class AssemblyBuilder : IAssemblyBuilder
    {
        public IReadOnlyDictionary<MDRecordKinds, ReferenceTable> ReferenceTables => mRefTables;
        public IReadOnlyDictionary<MDRecordKinds, DefinitionTable> DefinitionTables => mDefTables;
        public StringTable MetaStringTable => mStringTable;
        public AssemblyHeaderMD AssemblyHeader => mCurrentAssembly;
        public string AssemblyName => mStringTable.Contents[(int)AssemblyHeader.NameToken];
        public Guid AssemblyGuid => AssemblyHeader.GUID;
        public MDToken QueryFieldDefinition(string fullyQualifiedName)
            => FieldDefTable.GetDefinitionToken(fullyQualifiedName, () => new FieldMD());
        public MDToken QueryMethodDefinition(string fullyQualifiedName)
            => MethodDefTable.GetDefinitionToken(fullyQualifiedName, () => new MethodMD());
        public MDToken QueryTypeDefinition(string fullyQualifiedName)
            => TypeDefTable.GetDefinitionToken(fullyQualifiedName, () => new TypeMD());
        public MDToken TryDefineField(string fullyQualifiedName)
            => FieldDefTable.GetDefinitionToken(fullyQualifiedName, () => new FieldMD());
        public MDToken TryDefineMethod(string fullyQualifiedName)
            => MethodDefTable.GetDefinitionToken(fullyQualifiedName, () => new MethodMD());
        public MDToken TryDefineType(string fullyQualifiedName)
            => TypeDefTable.GetDefinitionToken(fullyQualifiedName, () => new TypeMD());
        public MDToken GetReferenceTokenOfType(string assembly, string fullyQualifiedName, MDToken defToken, MDRecordKinds kind = MDRecordKinds.TypeDef)
            => TypeReferenceTable.GetReferenceToken(fullyQualifiedName, () => new TypeRefMD()
            {
                DefKind = kind,
                AssemblyToken = string.IsNullOrEmpty(assembly) || assembly == AssemblyName ? 
                    AssemblyRefMD.Self : 
                    AssemblyReferenceTable.GetReferenceToken(assembly, null),
                Token = defToken
            });
    }
}
