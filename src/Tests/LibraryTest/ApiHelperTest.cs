using Library.Web.Results;

namespace Library.UnitTest;

[TestClass]
public class ApiHelperTest
{
    [TestMethod]
    public void ResultTest1()
    {
        var result = ApiResult.New(250);
        Assert.AreEqual(250, result?.Status);
    }

    [TestMethod]
    public void ResultTest2()
    {
        var result = ApiResult.New(207);
        Assert.AreEqual(true, result.IsSucceed);
    }

    [TestMethod]
    public void ResultTest3()
    {
        var result = ApiResult.New(250);
        Assert.AreEqual(true, result.IsSucceed);
    }

    [TestMethod]
    public void ResultTest4()
    {
        var result = ApiResult.Ok();
        Assert.AreEqual(true, result.IsSucceed);
    }
}
