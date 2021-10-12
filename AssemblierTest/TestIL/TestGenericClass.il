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
    .field public static [Core][Test]Hello<!T1>.World.This<!T1, int32> X

    .class nested public instance World
    {
        .class nested public instance This
        .generic T2 T3
        {

        }
    }
}