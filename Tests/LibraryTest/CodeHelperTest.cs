using Library.Exceptions;

namespace LibraryTest;

[TestClass]
public class FunctionalTests
{
    [TestMethod]
    [ExpectedException(typeof(BreakException))]
    public void BreakTest()
    {
        var zero = 0;
        if (zero == 0)
        {
            Break();
        }

        var invalid = 5 / zero;
    }
}
