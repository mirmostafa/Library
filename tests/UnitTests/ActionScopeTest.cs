using Library.Logging;
using Library.Results;

namespace UnitTests;


public class ActionScopeTest
{
    private readonly Func<Result> _failiureResult = () => Result.Failure;
    private readonly ILogger _logger = ILogger.Empty;
    private readonly Func<Result> _succeedResult = () => Result.Success;

    [Fact]
    public void BasicTest()
    {
        //ActionScope.Do(_logger, _succeedResult);
    }
}