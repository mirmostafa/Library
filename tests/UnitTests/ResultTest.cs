using System;
using System.Runtime.CompilerServices;

using Library.Exceptions.Validations;
using Library.Results;
using Library.Validations;

using Xunit;

namespace UnitTests;

public class ResultTest
{
    [Fact]
    public void _01_AddOperationResultCountTest()
    {
        var all = AddThreeResults();
        Assert.Equal(3, all.Errors?.Count());
    }

    [Fact]
    public void _02_AddOperationResultIndex1Test()
    {
        var all = AddThreeResults();
        Assert.Equal("Error One", all.Errors?.ElementAt(0).Error);
        Assert.Equal("Error Two", all.Errors?.ElementAt(1).Error);
        Assert.Equal("Error Thr", all.Errors?.ElementAt(2).Error);
    }

    [Fact]
    public void SimpleTest()
    {
        var five = Result<int>.CreateSuccess(5, 60);
        Assert.Equal(5, five);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Result<int> AddThreeResults()
    {
        var one = Result<int>.CreateFailure(1, 401, "One", error: (1, "Error One"));
        var two = Result<int>.CreateFailure(2, 402, "Two", error: (2, "Error Two"));
        var thr = Result<int>.CreateFailure(3, 403, "Thr", error: (3, "Error Thr"));
        var all = one + two + thr;
        return all;
    }

    [Fact]
    public void OnSuccessFalseTest()
    {
        var actual = Check(true, "1")
            .OnSucceed(() => Check(true, "2"))
            .OnSucceed(() => Check(false, "3"))
            .OnSucceed(() => Check(false, "4"));
        Assert.False(actual);
        Assert.Equal("3", actual.Message);
    }
    [Fact]
    public void OnSuccessTrueTest()
    {
        var actual = Check(true, "1")
            .OnSucceed(() => Check(true, "2"))
            .OnSucceed(() => Check(true, "3"))
            .OnSucceed(() => Check(true, "4"));
        Assert.True(actual);
        Assert.Equal("4", actual.Message);
    }
    [Fact]
    public void OnSuccessWithExceptionTest()
    {
        var actual = Record.Exception(() => Check(true, "1")
            .OnSucceed(() => Check(true, "2"))
            .OnSucceed(() => Check(false, "3"))
            .OnSucceed(() => Check(false, "4"))
            .ThrowOnFail());
        Assert.IsAssignableFrom<ValidationException>(actual);
        Assert.Equal("3", actual.Message);
    }

    static Result Check(bool isSucceed, string? message = null)
        => isSucceed ? Result.CreateSuccess(message: message) : Result.CreateFail(message: message);
}