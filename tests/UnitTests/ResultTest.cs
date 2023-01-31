﻿using Library.Results;

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
        var five = Add(2, 3);
        Assert.Equal(5, five);
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