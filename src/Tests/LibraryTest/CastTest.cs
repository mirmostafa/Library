using Library.UnitTest.Models;

namespace Library.UnitTest;

[TestClass]
public class CastTest
{
    [TestMethod]
    public void AsTest1()
    {
        StudentClass p = new();
        var s = p.As<PersonClass>();
        Assert.IsNotNull(s);
    }

    [TestMethod]
    public void AsTest2()
    {
        PersonClass p = new();
        var s = p.As<StudentClass>();
        Assert.IsNull(s);
    }
}