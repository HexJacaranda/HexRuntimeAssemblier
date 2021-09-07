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
        public MDToken QueryFieldReference(string fullQualifiedName)
            => FieldDefTable.GetDefinitionToken(fullQualifiedName, () => new FieldMD());
        public MDToken QueryMethodReference(string fullQualifiedName)
            => MethodDefTable.GetDefinitionToken(fullQualifiedName, () => new MethodMD());
        public MDToken QueryTypeReference(string fullQualifiedName)
            => TypeDefTable.GetDefinitionToken(fullQualifiedName, () => new TypeMD());
        public MDToken TryDefineField(string fullQualifiedName)
            => FieldDefTable.GetDefinitionToken(fullQualifiedName, () => new FieldMD());
        public MDToken TryDefineMethod(string fullQualifiedName)
            => MethodDefTable.GetDefinitionToken(fullQualifiedName, () => new MethodMD());
        public MDToken TryDefineType(string fullQualifiedName)
            => TypeDefTable.GetDefinitionToken(fullQualifiedName, () => new TypeMD());
        public MDToken GetReferenceTokenOfType(string fullQualifiedName)
            => mRefTables[MDRecordKinds.TypeRef].GetReferenceToken(fullQualifiedName, )
    }
}
