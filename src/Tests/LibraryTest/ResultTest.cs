using Library.Results;

namespace UnitTest;

[TestClass]
public class ResultTest
{
    [TestMethod]
    public void _01_AddOperationResultCountTest()
    {
        var all = AddThreeResults();
        Assert.AreEqual(3, all.Errors?.Count());
    }

    [TestMethod]
    public void _02_AddOperationResultIndex1Test()
    {
        var all = AddThreeResults();
        Assert.AreEqual("Error One", all.Errors?.ElementAt(0).Error);
        Assert.AreEqual("Error Two", all.Errors?.ElementAt(1).Error);
        Assert.AreEqual("Error Thr", all.Errors?.ElementAt(2).Error);
    }

    [TestMethod]
    public void SimpleTest()
    {
        var five = Add(2, 3);
        Assert.AreEqual(5, five);
    }

    private static Result<int> Add(int a, int b)
                => Result<int>.CreateSuccess(a + b);

    private static Result<int> AddThreeResults()
    {
        var one = Result<int>.CreateFail(1, 401, "One", error: (1, "Error One"));
        var two = Result<int>.CreateFail(2, 402, "Two", error: (2, "Error Two"));
        var thr = Result<int>.CreateFail(3, 403, "Thr", error: (3, "Error Thr"));
        var all = one + two + thr;
        return all;
    }
}