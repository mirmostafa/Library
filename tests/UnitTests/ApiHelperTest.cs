using Library.Web.Results;

namespace UnitTests;

[Trait("Category", "Helpers")]
public sealed class ApiHelperTest
{
    [Fact]
    public void ResultTest1()
    {
        var result = ApiResult.New(250);
        Assert.Equal(250, result?.Status);
    }

    [Fact]
    public void ResultTest2()
    {
        var result = ApiResult.New(207);
        Assert.True(result.IsSucceed);
    }

    [Fact]
    public void ResultTest3()
    {
        var result = ApiResult.New(250);
        Assert.True(result.IsSucceed);
    }

    [Fact]
    public void ResultTest4()
    {
        var result = ApiResult.Ok();
        Assert.True(result.IsSucceed);
    }
}