using System.Net;
using Library.Web.Results;

namespace LibraryTest;

[TestClass]
public class WebResultTest
{
    private static ApiResult Failed() => ApiResult.BadRequest();
    private static ApiResult Succeed() => ApiResult.Ok();

    [TestMethod]
    public void OkTest1()
    {
        var result = Succeed();
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void OkTest2()
    {
        var result = ApiResult<int>.Ok(5);
        Assert.AreEqual(5, result.Value);
    }

    [TestMethod]
    public void OkTest3()
    {
        var result = ApiResult.Ok(message: "Ok");
        Assert.AreEqual("Ok", result.Message);
    }

    [TestMethod]
    public void OkTest4()
    {
        var result = ApiResult<int>.Ok(message: "Ok", value: 5);
        Assert.AreEqual(5, result.Value);
    }

    [TestMethod]
    public void FailTest1()
    {
        var result = Failed();
        Assert.AreEqual(HttpStatusCode.BadRequest, result.HttpStatusCode);
    }

    [TestMethod]
    public void FluentTest1()
    {
        var num = -1;
        var result = Succeed()
                        .OnSucceed(x => num = 5)
                        .OnFailure(x => num = 6);
        Assert.AreEqual(5, num);
    }

    [TestMethod]
    public void FluentTest2()
    {
        var num = -1;
        var result = Failed()
                        .OnSucceed(r => num = 5)
                        .OnFailure(r => num = 6);
        Assert.AreEqual(6, num);
    }
    [TestMethod]
    public void FluentTest3()
    {
        var num = -1;
        var result = Failed()
                        .OnDone(r => num = 5);
        Assert.AreEqual(5, num);
    }
}
