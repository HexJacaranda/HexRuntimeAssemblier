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
    .method public instance void B() managed
    {
        .local
        {
        }
        .code
        {
            .ret
        }
    }
    .method public instance void .ctor() managed
    {
        .local
        {
            [0] int32& x
            [1] int64& y
        }
        .code
        {
            .call void Core.Hello::B()
            .pop
            .ret
        }
    }
    .method public instance void C() managed
    {
        .local
        {
        }
        .code
        {
            .ret
        }
    }
}