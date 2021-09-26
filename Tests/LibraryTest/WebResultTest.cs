using System.Net;
using Library.Web;

namespace LibraryTest;

[TestClass]
public class WebResultTest
{
    [TestMethod]
    public void OkTest1()
    {
        var result = WebResult.Ok();
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void OkTest2()
    {
        var result = WebResult<int>.Ok(5);
        Assert.AreEqual(5, result.Value);
    }

    [TestMethod]
    public void OkTest3()
    {
        var result = WebResult.Ok(message: "Ok");
        Assert.AreEqual("Ok", result.Message);
    }

    [TestMethod]
    public void OkTest4()
    {
        var result = WebResult<int>.Ok(message: "Ok", value: 5);
        Assert.AreEqual(5, result.Value);
    }

    [TestMethod]
    public void FailTest1()
    {
        var result = WebResult.BadRequest();
        Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
    }

    [TestMethod]
    public void ExtensionsTest1()
    {
        var num = -1;
        var result = succeed()
                        .OnSuccess(r => num = 5)
                        .OnFailure(r => num = 6);
        Assert.AreEqual(5, num);

        static WebResult succeed() => WebResult.Ok();
    }
    [TestMethod]
    public void ExtensionsTest2()
    {
        var num = -1;
        var result = failed()
                        .OnSuccess(r => num = 5)
                        .OnFailure(r => num = 6);
        Assert.AreEqual(6, num);

        static WebResult failed() => WebResult.BadRequest();
    }
}
