using HexRuntimeAssemblier.Interfaces;
using System.Collections.Generic;
using System;
using MDToken = System.UInt32;
namespace HexRuntimeAssemblier.Reference
{
    /// <summary>
    /// Responsible for resolving symbols from both internal or external assembly(s)
    /// </summary>
    public class GlobalResolver
    {
        public IReadOnlyDictionary<string, IAssemblyResolver> ExternalAssembly { get; }
        public IAssemblyBuilder CurrentAssembly { get; }
        public GlobalResolver(
            IAssemblyBuilder builder,
            IReadOnlyDictionary<string, IAssemblyResolver> externals)
        {
            CurrentAssembly = builder;
            ExternalAssembly = externals;
        }
        public MDToken QueryTypeDefinition(
            string assembly,
            string fullyQualifiedName)
        {
            if (string.IsNullOrEmpty(assembly) || assembly == CurrentAssembly.AssemblyName)
                return CurrentAssembly.TryDefineType(fullyQualifiedName);
            else
            {
                var external = ExternalAssembly[assembly];
                return external.QueryTypeDefinition(fullyQualifiedName);
            }
        }
        public MDToken QueryMethodDefinition(
            string assembly,
            string fullyQualifiedName)
        {
            if (string.IsNullOrEmpty(assembly) || assembly == CurrentAssembly.AssemblyName)
                return CurrentAssembly.TryDefineMethod(fullyQualifiedName);
            else
            {
                var external = ExternalAssembly[assembly];
                return external.QueryMethodDefinition(fullyQualifiedName);
            }
        }
        public MDToken QueryFieldDefinition(
            string assembly,
            string fullyQualifiedName)
        {
            if (string.IsNullOrEmpty(assembly) || assembly == CurrentAssembly.AssemblyName)
                return CurrentAssembly.TryDefineField(fullyQualifiedName);
            else
            {
                var external = ExternalAssembly[assembly];
                return external.QueryFieldDefinition(fullyQualifiedName);
            }
        }
        public MDToken QueryTypeReference(
            string assembly,
            string fullyQualifiedName,
            Func<MDToken, MDToken> referenceTokenGenerator)
            => referenceTokenGenerator(QueryTypeDefinition(assembly, fullyQualifiedName));

        public MDToken QueryMethodReference(
            string assembly,
            string fullyQualifiedName,
            Func<MDToken, MDToken> referenceTokenGenerator)
            => referenceTokenGenerator(QueryMethodDefinition(assembly, fullyQualifiedName));
    }
}
