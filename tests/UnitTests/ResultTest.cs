﻿using System.Runtime.CompilerServices;

using Library.Results;

namespace UnitTests;

[Trait("Category", nameof(Library.Results))]
[Trait("Category", nameof(Result))]
public sealed class ResultTest
{
    [Fact]
    public void AddOperationResultCountTest()
    {
        var all = AddThreeResults();
        Assert.Equal(5, all.Errors.Length);
        Assert.Equal(403, all.Status);
        Assert.Equal((3, "Error Thr"), all.Errors.Last());
    }

    [Fact]
    public void AddOperationResultIndex1Test()
    {
        var all = AddThreeResults();
        Assert.Equal((1, "Error One"), all.Errors.ElementAt(0));
        Assert.Equal((2, "Error Two"), all.Errors.ElementAt(1));
        Assert.Equal((null!, 401), all.Errors.ElementAt(2));
        Assert.Equal((null!, 402), all.Errors.ElementAt(3));
        Assert.Equal((3, "Error Thr"), all.Errors.ElementAt(4));
    }

    [Fact]
    public void OnSuccessFalseTest()
    {
        var actual = Check(true, "1")
            .IfSucceed(() => Check(true, "2"))
            .IfSucceed(() => Check(false, "3"))
            .IfSucceed(() => Check(false, "4"));
        Assert.False(actual);
        Assert.Equal("3", actual.Message);
    }

    [Fact]
    public void OnSuccessTrueTest()
    {
        var actual = Check(true, "1")
            .IfSucceed(() => Check(true, "2"))
            .IfSucceed(() => Check(true, "3"))
            .IfSucceed(() => Check(true, "4"));
        Assert.True(actual);
        Assert.Equal("4", actual.Message);
    }

    [Fact]
    public void SimpleTest()
    {
        int five = Result<int>.CreateSuccess(5, 60);
        Assert.Equal(5, five);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Result<int> AddThreeResults()
    {
        var one = Result<int>.CreateFailure(value: 1, status: 401, message: "One", error: (1, "Error One"));
        var two = Result<int>.CreateFailure(value: 2, status: 402, message: "Two", error: (2, "Error Two"));
        var thr = Result<int>.CreateFailure(value: 3, status: 403, message: "Thr", error: (3, "Error Thr"));
        var all = one + two + thr;
        return all;
    }

    private static Result Check(bool isSucceed, string? message = null)
        => isSucceed ? Result.CreateSuccess(message: message) : Result.CreateFailure(message: message);
}