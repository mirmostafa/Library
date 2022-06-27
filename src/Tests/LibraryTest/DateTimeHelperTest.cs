namespace UnitTest;

[TestClass]
public class DateTimeHelperTestClass
{
    [TestMethod]
    public void Deconstruct1()
    {
        var (year, month, day, hour, minute, second, ms) = DateTime.MaxValue;
        Assert.AreEqual(year, 9999);
        Assert.AreEqual(month, 12);
        Assert.AreEqual(day, 31);
        Assert.AreEqual(hour, 23);
        Assert.AreEqual(minute, 59);
        Assert.AreEqual(second, 59);
        Assert.AreEqual(ms, 999);
    }

    [TestMethod]
    public void Deconstruct2()
    {
        var (year, month, day) = DateTime.MaxValue;
        Assert.AreEqual(year, 9999);
        Assert.AreEqual(month, 12);
        Assert.AreEqual(day, 31);
    }

    [TestMethod]
    public void Deconstruct3()
    {
        var (hour, minute, second, ms) = DateTime.MaxValue;
        Assert.AreEqual(hour, 23);
        Assert.AreEqual(minute, 59);
        Assert.AreEqual(second, 59);
        Assert.AreEqual(ms, 999);
    }
}