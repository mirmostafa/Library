using Library.Logging;
using System.Diagnostics;

namespace LibraryTest;
[TestClass]
public class CodeBlockLoggerTest
{
    [TestMethod]
    public void TestMethod1()
    {
        var number = 5;
        VsOutputLogger logger = new();
        using (CodeBlockLogger.New(logger, "~~~~Get add 6 to a number…", "~~~~~Got."))
        {
            number += 6;
        }
    }
}
