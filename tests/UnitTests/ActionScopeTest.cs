//using Library.Coding;
//using Library.Logging;
//using Library.Results;

//namespace UnitTests;

//public class ActionScopeTest
//{
//    [Fact]
//    public void FundamentalTest()
//    {
//        var result = ILogger.Empty.ActionScopeBegin("Processing...")
//            .OnSucceed("Ready")
//            .OnFailure(x => x.GetBaseException().Message)
//            .Run(this.SimpleMethod)
//            .ThrowOnFail(nameof(ActionScopeTest));
//    }

//    private void SimpleMethod()
//    { }

//    private Result<int> SimpleMethodFailureGenericResult() => Result<int>.CreateFailure();

//    private Result SimpleMethodFailureResult() => Result.Failure;

//    private Result<int> SimpleMethodSuccessGenericResult() => Result<int>.CreateSuccess(5);

//    private Result SimpleMethodSuccessResult() => Result.Success;

//    private void SimpleMethodWithException()
//    { }
//}