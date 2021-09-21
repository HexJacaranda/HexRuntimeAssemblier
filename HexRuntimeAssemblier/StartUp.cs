using HexRuntimeAssemblier.Interfaces;
using HexRuntimeAssemblier.Reference;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace HexRuntimeAssemblier
{
    static class HexRuntimeAssemblierExtensions
    {
        public static IServiceCollection AddBuildConfiguration(this IServiceCollection services, string json)
        {
            var configuration = JsonSerializer.Deserialize<AssemblyBuildConfiguration>(json);
            return services.AddSingleton(configuration)
                .AddSingleton(configuration.Options);
        }
        public static IServiceCollection AddAssemblyConstant(this IServiceCollection services)
            => services.AddSingleton(CoreAssemblyConstant.Default);
        public static IServiceCollection AddAssemblyResolvers(this IServiceCollection services)
            => services.AddSingleton<IEnumerable<IAssemblyResolver>>(x =>
            {
                var configuration = x.GetRequiredService<AssemblyBuildConfiguration>();
                return configuration.References.Select(u => AssemblyResolver.Build(File.OpenRead(u))).ToList();
            });
        public static IServiceCollection AddAssemblyBuilder(this IServiceCollection services)
            => services
                .AddTransient<IAssemblyBuilder, AssemblyBuilder>()
                .AddSingleton<IAssemblyBuilderFactory, AssemblyBuilderFactory>();
    }
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var provider = BuildService(args[0]);
            var program = provider.GetService<AssemblierProgram>();
            program.Run();
        }

        public static IServiceProvider BuildService(string configurationJsonPath)
        {
            string compileConfiguration = File.ReadAllText(configurationJsonPath);

            var services = new ServiceCollection()
                .AddLogging(x => x.AddConsole())
                .AddBuildConfiguration(compileConfiguration)
                .AddAssemblyConstant()
                .AddAssemblyResolvers()
                .AddAssemblyBuilder()
                .AddSingleton<AssemblierProgram>();

            return services.BuildServiceProvider();
        }

    }
}
