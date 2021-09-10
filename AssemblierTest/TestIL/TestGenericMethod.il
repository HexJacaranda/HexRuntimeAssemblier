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
.generic T1
{
    .method public instance !T1 A() managed
    .generic T2
    {
        .code 
        {
            .new !T2 !T2::.ctor()
            .conv int32
            .ret
        }
    }
    
    .method public instance void B() managed
    {
        .code
        {
            .call !T1 [Test]Hello<int64>::A<int32>()
            .pop
            .ret
        }
    }

}