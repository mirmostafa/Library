﻿using Library.Results;

namespace Library.UnitTest;

[TestClass]
public class ValidationHelperTest
{
    [TestMethod]
    public void _01_ValidateAllTrueTest()
    {
        var result = Check("Starts and End");
        Assert.IsTrue(result.IsSucceed);
    }

    [TestMethod]
    public void _02_ValidateAllFalseTest()
    {
        var result = Check("Starts and Ends");
        Assert.IsFalse(result.IsSucceed);
    }

    private static Result<string> Check(string INPUT)
    {
        Result<string> startsWithStart(string x)
            => x.StartsWith("Start")
                ? Result<string>.CreateSuccess(INPUT)
                : Result<string>.CreateFail("Not started with 'Start'", 2, INPUT)!;
        Result<string> endsWithStart(string x)
            => x.EndsWith("End")
                ? Result<string>.CreateSuccess(INPUT)
                : Result<string>.CreateFail("Not ended with 'End'", 2, INPUT)!;
        return ValidationHelper.CheckAll(INPUT, startsWithStart, endsWithStart);
    }
}