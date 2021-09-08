using HexRuntimeAssemblier.Interfaces;
using HexRuntimeAssemblier.Meta;
using System.Collections.Generic;
using MDToken = System.UInt32;

namespace HexRuntimeAssemblier
{
    public partial class AssemblyBuilder : IAssemblyBuilder
    {
        public IReadOnlyDictionary<MDRecordKinds, ReferenceTable> ReferenceTables => mRefTables;
        public IReadOnlyDictionary<MDRecordKinds, DefinitionTable> DefinitionTables => mDefTables;
        public StringTable MetaStringTable => mStringTable;
        public AssemblyHeaderMD AssemblyHeader => mCurrentAssembly;
        public MDToken QueryFieldDefinition(string fullQualifiedName)
            => FieldDefTable.GetDefinitionToken(fullQualifiedName, () => new FieldMD());
        public MDToken QueryMethodDefinition(string fullQualifiedName)
            => MethodDefTable.GetDefinitionToken(fullQualifiedName, () => new MethodMD());
        public MDToken QueryTypeDefinition(string fullQualifiedName)
            => TypeDefTable.GetDefinitionToken(fullQualifiedName, () => new TypeMD());
        public MDToken TryDefineField(string fullQualifiedName)
            => FieldDefTable.GetDefinitionToken(fullQualifiedName, () => new FieldMD());
        public MDToken TryDefineMethod(string fullQualifiedName)
            => MethodDefTable.GetDefinitionToken(fullQualifiedName, () => new MethodMD());
        public MDToken TryDefineType(string fullQualifiedName)
            => TypeDefTable.GetDefinitionToken(fullQualifiedName, () => new TypeMD());
        public MDToken GetReferenceTokenOfType(string assembly, string fullQualifiedName, MDToken defToken)
            => TypeReferenceTable.GetReferenceToken(fullQualifiedName, () => new TypeRefMD()
            {
                DefKind = MDRecordKinds.TypeRef,
                AssemblyToken = string.IsNullOrEmpty(assembly) ? AssemblyRefMD.Self : AssemblyReferenceTable.GetReferenceToken(assembly, null),
                Token = defToken
            });
    }
}
