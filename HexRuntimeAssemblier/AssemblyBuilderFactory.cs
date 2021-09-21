using HexRuntimeAssemblier.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace HexRuntimeAssemblier
{
    public class AssemblyBuilderFactory : IAssemblyBuilderFactory
    {
        private readonly IServiceProvider mServiceProvider;
        public AssemblyBuilderFactory(IServiceProvider provider)
        {
            mServiceProvider = provider;
        }
        public IAssemblyBuilder Create()
            => mServiceProvider.GetService<IAssemblyBuilder>();
    }
}
