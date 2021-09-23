using HexRuntimeAssemblier.Interfaces;
using HexRuntimeAssemblier.Serialization;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Collections.Generic;
using System;

namespace HexRuntimeAssemblier
{
    public class AssemblierProgram
    {
        private readonly ILogger<AssemblierProgram> mLog;
        private readonly AssemblyBuildConfiguration mConfiguration;
        private readonly IAssemblyBuilderFactory mBuilderFactory;
        public AssemblierProgram(
            ILogger<AssemblierProgram> logger,
            AssemblyBuildConfiguration configuration,
            IAssemblyBuilderFactory builderFactory)
        {
            mLog = logger;
            mConfiguration = configuration;
            mBuilderFactory = builderFactory;
        }
        public void Run()
        {
            List<Exception> errors = new();
            foreach (var input in mConfiguration.Inputs)
            {
                mLog.LogInformation($"Begin compiling assembly {input}");
                try
                {
                    using var file = File.OpenRead(input);
                    mLog.LogInformation("Building relations from il file");
                    var builder = mBuilderFactory.Create().Build(file);

                    var outputPath = Path.Combine(mConfiguration.OutputDirectory, builder.AssemblyName);
                    using var outputFile = File.OpenWrite(outputPath);
                    mLog.LogInformation("Serializing assembly to binary");
                    new AssemblySerializer(outputFile, builder).Serialize();
                }
                catch (Exception ex)
                {
                    mLog.LogCritical(ex, $"Assemblier encounters unexpected exception when compiling [{input}]");
                    errors.Add(ex);
                }
            }
            if (errors.Count > 0)
                throw new AggregateException(errors);
        }
    }
}
