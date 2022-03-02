using Library.Coding;
using Library.Exceptions;

namespace LibraryTest;

[TestClass]
public class CodeHelperTest
{
    [TestMethod]
    [ExpectedException(typeof(BreakException))]
    public void BreakTest()
    {
        var zero = 0;
        if (zero == 0)
        {
            CodeHelper.Break();
        }

        var invalid = 5 / zero;
    }
}
