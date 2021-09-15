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
        /// Defines alias to QFN of primitive types
        /// </summary>
        public IReadOnlyDictionary<string, string> PrimitiveTypes { get; init; }
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
        /// <summary>
        /// Provide a standard mapping for building
        /// </summary>
        public static CoreAssemblyConstant Default { get; } = new CoreAssemblyConstant()
        {
            PrimitiveTypes = new Dictionary<string, string>()
            {
                ["bool"] = "[System]Boolean",
                ["char"] = "[System]Char",
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
            PrimitiveToCore = new Dictionary<string, CoreTypes>()
            {
                ["[System]Boolean"] = CoreTypes.I1,
                ["[System]Int8"] = CoreTypes.I1,
                ["[System]Int16"] = CoreTypes.I2,
                ["[System]Int32"] = CoreTypes.I4,
                ["[System]Int64"] = CoreTypes.I8,
                ["[System]UInt8"] = CoreTypes.U1,
                ["[System]UInt16"] = CoreTypes.U2,
                ["[System]UInt32"] = CoreTypes.U4,
                ["[System]UInt64"] = CoreTypes.U8,
                ["[System]Float"] = CoreTypes.R4,
                ["[System]Double"] = CoreTypes.R8,
                ["[System]String"] = CoreTypes.String,
                ["[System]Object"] = CoreTypes.Object
            },
            CanonicalPlaceholder = "Canon",
            Array = "[System]Array",
            InteriorReference = "[System]Interior",
            AssemblyStandardName = "Core"
        };
    }
}
