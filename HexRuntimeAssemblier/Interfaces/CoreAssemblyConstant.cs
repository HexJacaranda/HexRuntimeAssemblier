using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HexRuntimeAssemblier.IL;

namespace HexRuntimeAssemblier.Interfaces
{
    /// <summary>
    /// Defines type aliases of core assembly
    /// </summary>
    public class CoreAssemblyConstant
    {
        /// <summary>
        /// Defines alias to FQN of primitive types
        /// </summary>
        public IReadOnlyDictionary<string, string> PrimitiveTypes { get; init; }
        /// <summary>
        /// Defines mapping from FQN to core type
        /// </summary>
        public IReadOnlyDictionary<string, CoreTypes> PrimitiveToCore { get;init; }
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
        public string GlobalNamespace { get; init; }
        /// <summary>
        /// Provide a standard mapping for building
        /// </summary>
        public static CoreAssemblyConstant Default { get; } = new CoreAssemblyConstant()
        {
            PrimitiveTypes = new Dictionary<string, string>()
            {
                ["bool"] = "[Core][global]Boolean",
                ["char"] = "[Core][global]Char",
                ["int8"] = "[Core][global]Int8",
                ["int16"] = "[Core][global]Int16",
                ["int32"] = "[Core][global]Int32",
                ["int64"] = "[Core][global]Int64",
                ["uint8"] = "[Core][global]UInt8",
                ["uint16"] = "[Core][global]UInt16",
                ["uint32"] = "[Core][global]UInt32",
                ["uint64"] = "[Core][global]UInt64",
                ["float"] = "[Core][global]Float",
                ["double"] = "[Core][global]Double",
                ["string"] = "[Core][global]String",
                ["object"] = "[Core][global]Object",
            },
            PrimitiveToCore = new Dictionary<string, CoreTypes>()
            {
                ["[Core][global]Boolean"] = CoreTypes.I1,
                ["[Core][global]Int8"] = CoreTypes.I1,
                ["[Core][global]Int16"] = CoreTypes.I2,
                ["[Core][global]Int32"] = CoreTypes.I4,
                ["[Core][global]Int64"] = CoreTypes.I8,
                ["[Core][global]UInt8"] = CoreTypes.U1,
                ["[Core][global]UInt16"] = CoreTypes.U2,
                ["[Core][global]UInt32"] = CoreTypes.U4,
                ["[Core][global]UInt64"] = CoreTypes.U8,
                ["[Core][global]Float"] = CoreTypes.R4,
                ["[Core][global]Double"] = CoreTypes.R8,
                ["[Core][global]String"] = CoreTypes.String,
                ["[Core][global]Object"] = CoreTypes.Object
            },
            CanonicalPlaceholder = "Canon",
            Array = "[Core][global]Array",
            InteriorReference = "[Core][global]Interior",
            AssemblyStandardName = "Core",
            GlobalNamespace = "global"
        };
    }
}
