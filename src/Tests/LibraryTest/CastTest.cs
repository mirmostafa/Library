using UnitTest.Models;

namespace UnitTest;

[TestClass]
public class CastTest
{
    [TestMethod]
    public void AsTest1()
    {
        StudenClass p = new();
        var s = Cast.As<PersonClass>(p);
        Assert.IsNotNull(s);
    }

    [TestMethod]
    public void AsTest2()
    {
        PersonClass p = new();
        var s = Cast.As<StudenClass>(p);
        Assert.IsNull(s);
    }
}