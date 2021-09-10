using System.Text.Json.Serialization;

namespace HexRuntimeAssemblier
{
    public class AssemblyOptions
    {
        [JsonPropertyName("core_library")]
        public bool CoreLibrary { get; set; }
        /// <summary>
        /// If this is enabled, all the core types will not be resovled,
        /// which is usually used in test
        /// </summary>
        [JsonPropertyName("disable_core_type")]
        public bool DisableCoreType { get; set; }
    }
    public class AssemblyBuildConfiguration
    {
        [JsonPropertyName("inputs")]
        public string[] Inputs { get; set; }
        [JsonPropertyName("references")]
        public string[] References { get; set; }
        [JsonPropertyName("output_directory")]
        public string OutputDirectory { get; set; }
        [JsonPropertyName("options")]
        public AssemblyOptions Options { get; set; }
    }
}
