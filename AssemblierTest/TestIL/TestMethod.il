.assembly
{
    name = "Core"
    major = 1
    minor = 0
    groupname = "HexRT"
    guid = 00000000-0000-0000-0000-000000000000
}

.class public instance Hello
.namespace Test
{
    .method public instance void B() managed
    {
        .code
        {
            .ret
        }
    }
    .method public instance void .ctor() managed
    {
        .local
        {
            int32& x
            int64& y
        }
        .code
        {
            .call [Core][Test]Hello::C()
            .pop
            .ret
        }
    }
    .method public instance void C() managed
    {
        .code
        {
            .ret
        }
    }
}