namespace HexRuntimeAssemblier.IL
{
	public enum CoreTypes : byte
	{
		Bool = 0x00,
		Char = 0x01,

		I1 = 0x10,
		I2 = 0x11,
		I4 = 0x12,
		I8 = 0x13,

		U1 = 0x14,
		U2 = 0x15,
		U4 = 0x16,
		U8 = 0x17,

		/// <summary>
		/// Half
		/// </summary>
		R2 = 0x18,
		/// <summary>
		/// Single
		/// </summary>
		R4 = 0x19,
		/// <summary>
		/// Double
		/// </summary>
		R8 = 0x1A,
		/// <summary>
		/// Custom structure
		/// </summary>
		Struct = 0x2F,

		/// <summary>
		/// Pointer
		/// </summary>
		Ref = 0x3C,

		/// <summary>
		/// Interior pointer
		/// </summary>
		InteriorRef = 0x3D,

		//Detailed type representation
		Object = 0x40,
		SZArray = 0x41,
		Array = 0x42,
		String = 0x43,
		Delegate = 0x44
	}
}
