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
            string fullQualifiedName)
        {
            if (string.IsNullOrEmpty(assembly))
                return CurrentAssembly.TryDefineType(fullQualifiedName);
            else
            {
                var external = ExternalAssembly[assembly];
                return external.QueryTypeDefinition(fullQualifiedName);
            }
        }
        public MDToken QueryMethodDefinition(
            string assembly,
            string fullQualifiedName)
        {
            if (string.IsNullOrEmpty(assembly))
                return CurrentAssembly.TryDefineMethod(fullQualifiedName);
            else
            {
                var external = ExternalAssembly[assembly];
                return external.QueryMethodDefinition(fullQualifiedName);
            }
        }
        public MDToken QueryFieldDefinition(
            string assembly,
            string fullQualifiedName)
        {
            if (string.IsNullOrEmpty(assembly))
                return CurrentAssembly.TryDefineField(fullQualifiedName);
            else
            {
                var external = ExternalAssembly[assembly];
                return external.QueryFieldDefinition(fullQualifiedName);
            }
        }
        public MDToken QueryTypeReference(
            string assembly,
            string fullQualifiedName,
            Func<MDToken, MDToken> referenceTokenGenerator)
            => referenceTokenGenerator(QueryTypeDefinition(assembly, fullQualifiedName));
        public MDToken QueryMethodReference(
            string assembly,
            string fullQualifiedName,
            Func<MDToken, MDToken> referenceTokenGenerator)
            => referenceTokenGenerator(QueryMethodDefinition(assembly, fullQualifiedName));
    }
}
