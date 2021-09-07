using System;
using System.Collections.Generic;
using HexRuntimeAssemblier.Meta;

using MDToken = System.UInt32;

namespace HexRuntimeAssemblier.Interfaces
{
    /// <summary>
    /// Resolve definition token according to full qualified name
    /// </summary>
    public interface IAssemblyResolver
    {
        MDToken QueryTypeReference(string fullQualifiedName);
        MDToken QueryMethodReference(string fullQualifiedName);
        MDToken QueryFieldReference(string fullQualifiedName);
    }

    /// <summary>
    /// Assembly builder
    /// </summary>
    public interface IAssemblyBuilder : IAssemblyResolver
    {
        MDToken TryDefineType(string fullQualifiedName);
        MDToken TryDefineMethod(string fullQualifiedName);
        MDToken TryDefineField(string fullQualifiedName);
        StringTable MetaStringTable { get; }
        IReadOnlyDictionary<MDRecordKinds, ReferenceTable> ReferenceTables { get; }
        IReadOnlyDictionary<MDRecordKinds, DefinitionTable> DefinitionTables { get; }
    }
}
