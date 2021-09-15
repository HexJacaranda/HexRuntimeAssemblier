namespace HexRuntimeAssemblier.IL
{
    public enum CoreTypes : byte
    {
        I1, I2, I4, I8,
        U1, U2, U4, U8,
        R2, R4, R8,
        Struct,
        Ref,
        InteriorRef,
        Object = 0x10,
        SZArray = 0x11,
        Array = 0x12,
        String = 0x13,
        Delegate = 0x14
    }
}
