.assembly
{
    name = "Reference"
    major = 1
    minor = 0
    groupname = "HexRT"
    guid = 00000000-0000-0000-0000-000000000001
}

.class public instance Hello
.namespace Test
.generic T1
{
    .field public static [Reference][Test]Hello<!T1>.World.This<!T1, int32> X
    .field public static [Core]Double Y

    .class nested public instance World
    {
        .class nested public instance This
        .generic T2 T3
        {

        }
    }

    .method public instance void A(int32& outValue) managed
    {
        .code
        {
            .ret
        }
    }

    .method public instance void B() managed
    {
        .code
        {
            .ldc int32 2
            .pop
            .ret
        }
    }

    .method public instance object C() managed
    {
        .code
        {
            .ldc int32 2
            .pop
            .ret
        }
    }

    .method public instance void Main(array<string> args) managed
    {
        .code
        {
            .ret
        }
    }
}
