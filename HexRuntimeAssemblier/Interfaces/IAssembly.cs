using System;
using System.IO;
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
        string AssemblyName { get; }
        Guid AssemblyGuid { get; }
        MDToken QueryTypeDefinition(string fullyQualifiedName);
        MDToken QueryMethodDefinition(string fullyQualifiedName);
        MDToken QueryFieldDefinition(string fullyQualifiedName);
    }

    /// <summary>
    /// Assembly builder
    /// </summary>
    public interface IAssemblyBuilder : IAssemblyResolver
    {
        MDToken TryDefineType(string fullyQualifiedName);
        MDToken TryDefineMethod(string fullyQualifiedName);
        MDToken TryDefineField(string fullyQualifiedName);
        StringTable MetaStringTable { get; }
        ReferenceTable AssemblyReferenceTable { get; }
        AssemblyHeaderMD AssemblyHeader { get; }
        IReadOnlyDictionary<MDRecordKinds, ReferenceTable> ReferenceTables { get; }
        IReadOnlyDictionary<MDRecordKinds, DefinitionTable> DefinitionTables { get; }
        IAssemblyBuilder Build(Stream originStream);
    }
}
