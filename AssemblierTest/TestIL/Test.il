.assembly
{
    name = "Core"
    major = 1
    minor = 0
    groupname = "HexRT"
    guid = 00000000-0000-0000-0000-000000000000
}

.class public instance Core.Hello
{
    .method public instance void .ctor() managed
    {
        .local
        {
            [0] int32& x
            [1] int64& y
        }
        .code
        {
            .ret int32 void Core.Sys::Fun(int32)
            .push int32 3
            .pop
        }
    }
}