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
        public MDToken QueryReference(
            string assembly,
            string fullQualifiedName,
            Func<MDToken, MDToken> referenceTokenGenerator)
        {
            if (string.IsNullOrEmpty(assembly))
            {
                //Reference to type inside current assembly
                return referenceTokenGenerator(CurrentAssembly.TryDefineType(fullQualifiedName));
            }
            else
            {
                var external = ExternalAssembly[assembly];
                var defToken = external.QueryTypeReference(fullQualifiedName);
                return referenceTokenGenerator(defToken);
            }
        }
    }
}
