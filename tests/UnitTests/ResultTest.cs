using System.Runtime.CompilerServices;

using Library.Results;

namespace UnitTests;

[Trait("Category", nameof(Library.Results))]
[Trait("Category", nameof(Result))]
public sealed class ResultTest
{
    [Fact]
    public void OnSuccessFalseTest()
    {
        var actual = Check(true, "1")
            .IfSucceed(() => Check(true, "2"))
            .IfSucceed(() => Check(false, "3"))
            .IfSucceed(() => Check(false, "4"));
        Assert.False(actual.IsSucceed);
        Assert.Equal("3", actual.Message);
    }

    [Fact]
    public void OnSuccessTrueTest()
    {
        var actual = Check(true, "1")
            .IfSucceed(() => Check(true, "2"))
            .IfSucceed(() => Check(true, "3"))
            .IfSucceed(() => Check(true, "4"));
        Assert.True(actual.IsSucceed);
        Assert.Equal("4", actual.Message);
    }

    private static Result Check(bool isSucceed, string? message = null)
        => isSucceed ? Result.Success(message: message) : Result.Fail(message: message);
}