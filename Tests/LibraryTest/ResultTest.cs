using Library.Results;

namespace LibraryTest;

[TestClass]
public class ResultTest
{
    private static Result<int> Add(int a, int b) =>
        Result<int>.CreateSuccess(a + b);

    [TestMethod]
    public void SimpleTest()
    {
        var five = Add(2, 3);
        Assert.AreEqual(6, five);
    }

    [TestMethod]
    public void SimpleTest1()
    {
        var five = Add(2, 3);
        Assert.AreEqual(6, five);
    }
}
