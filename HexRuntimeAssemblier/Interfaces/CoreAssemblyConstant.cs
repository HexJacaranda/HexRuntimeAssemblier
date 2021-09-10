using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexRuntimeAssemblier.Interfaces
{
    /// <summary>
    /// Defines type aliases of core assembly
    /// </summary>
    public class CoreAssemblyConstant
    {
        /// <summary>
        /// Defines alias to QFN of primitive types
        /// </summary>
        public IReadOnlyDictionary<string, string> PrimitiveTypes { get; init; }
        /// <summary>
        /// Array type name
        /// </summary>
        public string Array { get; init; }
        /// <summary>
        /// Multidimensional array type name
        /// </summary>
        public string MultidimensionalArray { get; init; }
        /// <summary>
        /// Interior reference type name
        /// </summary>
        public string InteriorReference { get; init; }
        /// <summary>
        /// The symbol used for canonical type parameter placeholder
        /// </summary>
        public string CanonicalPlaceholder { get; init; }
        /// <summary>
        /// The name of core assembly, usually "Core"
        /// </summary>
        public string AssemblyStandardName { get; init; }
        /// <summary>
        /// Provide a standard mapping for building
        /// </summary>
        public static CoreAssemblyConstant Default { get; } = new CoreAssemblyConstant()
        {
            PrimitiveTypes = new Dictionary<string, string>()
            {
                ["bool"] = "[System]Boolean",
                ["int8"] = "[System]Int8",
                ["int16"] = "[System]Int16",
                ["int32"] = "[System]Int32",
                ["int64"] = "[System]Int64",
                ["uint8"] = "[System]UInt8",
                ["uint16"] = "[System]UInt16",
                ["uint32"] = "[System]UInt32",
                ["uint64"] = "[System]UInt64",
                ["float"] = "[System]Float",
                ["double"] = "[System]Double",
                ["string"] = "[System]String",
                ["object"] = "[System]Object",
            },
            CanonicalPlaceholder = "Canon",
            Array = "[System]Array",
            InteriorReference = "[System]Interior",
            AssemblyStandardName = "Core"
        };
    }
}
