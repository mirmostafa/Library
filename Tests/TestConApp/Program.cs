using Library.Logging;

namespace TestConApp;

internal partial class Program
{
    private static void Main(params string[] args)
    {
        _logger.Info("This is info.");
        _logger.Debug("This is debug.");
        _logger.Error("This is error.");
        _logger.Warn("This is warning.");
        Test();

        var w = new DbCrudLogger(_logger, "Console App", 5000);
        w.ItemAdded("apple (1027)");
        w.ItemUpdated("apple (1027)");
    }

    private static void Test() => _logger.Info("1");
}