namespace UnitTest;

[TestClass]
public class CommonHelpersTestClass
{
    [TestMethod]
    public void BooleanHelper_Any()
    {
        var expected = true;
        var actual = BooleanHelper.Any(true, true, true);
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void BooleanHelper_Any01()
    {
        var expected = true;
        var actual = BooleanHelper.Any(true, false, true);
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void BooleanHelper_Any02()
    {
        var expected = true;
        var actual = BooleanHelper.Any(false, false, false);
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void BooleanHelper_Any03()
    {
        var expected = false;
        var actual = BooleanHelper.Any();
        Assert.AreEqual(expected, actual);
    }
}