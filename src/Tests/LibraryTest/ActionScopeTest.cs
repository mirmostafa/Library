using Library.Logging;
using Library.Results;

namespace Library.UnitTest;

[TestClass]
public class ActionScopeTest
{
    private readonly Func<Result> _failiureResult = () => Result.Fail;
    private readonly ILogger _logger = ILogger.Empty;
    private readonly Func<Result> _succeedResult = () => Result.Success;

    [TestMethod]
    public void BasicTest()
    {
        //ActionScope.Do(_logger, _succeedResult);
    }
}