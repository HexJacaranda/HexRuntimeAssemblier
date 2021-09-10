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
    .method .get public instance int32 getX() managed
    {
        .code
        {
            .ldc int32 2
            .ret
        }
    }
    .method .set public instance void setX(int32 value) managed
    {
        .code
        {
            .ret
        }
    }
    .property int32 X
    {
        .get [Test]Hello::getX()
        .set [Test]Hello::setX(int32)
    }
}