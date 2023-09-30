using System.Net;

using Library.Web.Helpers;
using Library.Web.Results;

namespace UnitTests;


[Trait("Category", "Web")]
public sealed class WebResultTest
{
    private static ApiResult Failed() 
        => ApiResult.BadRequest();
    private static ApiResult Succeed() 
        => ApiResult.Ok();

    [Fact]
    public void OkTest1()
    {
        var result = Succeed();
        Assert.NotNull(result);
    }

    [Fact]
    public void OkTest2()
    {
        var result = ApiResult<int>.Ok(5);
        Assert.Equal(5, result.Value);
    }

    [Fact]
    public void OkTest3()
    {
        var result = ApiResult.Ok(message: "Ok");
        Assert.Equal("Ok", result.Message);
    }

    [Fact]
    public void OkTest4()
    {
        var result = ApiResult<int>.Ok(message: "Ok", value: 5);
        Assert.Equal(5, result.Value);
    }

    [Fact]
    public void FailTest1()
    {
        var result = Failed();
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatusCode);
    }

    [Fact]
    public void FluentTest1()
    {
        var num = -1;
        var result = Succeed()
                        .IfSucceed(x => num = 5)
                        .IfFailure(x => num = 6);
        Assert.Equal(5, num);
    }

    [Fact]
    public void FluentTest2()
    {
        var num = -1;
        var result = Failed()
                        .IfSucceed(r => num = 5)
                        .IfFailure(r => num = 6);
        Assert.Equal(6, num);
    }
    [Fact]
    public void FluentTest3()
    {
        var num = -1;
        var result = Failed()
                        .OnDone(r => num = 5);
        Assert.Equal(5, num);
    }
}
