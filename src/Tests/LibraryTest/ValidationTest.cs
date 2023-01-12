using Library.Exceptions.Validations;
using Library.Validations;

namespace UnitTest;

[TestClass]
public class ValidationTest
{
    [TestMethod]
    public void _01_BeNotNullNotNullTest()
    {
        var arg = "Test";

        arg = arg.Should().BeNotNull().ThrowOnFail();
    }

    [TestMethod]
    [ExpectedException(typeof(NullValueValidationException))]
    public void _02_BeNotNullNullTest()
    {
        string? arg = null;

        arg = arg.Should().BeNotNull().Build();
    }

    [TestMethod]
    public void _03_ArgumentBeNotNullNotNullTest()
    {
        var arg = "Test";

        arg = arg.Should().ArgumentBeNotNull().ThrowOnFail();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void _04_ArgumentBeNotNullNullTest()
    {
        string? arg = null;

        arg = arg.Should().ArgumentBeNotNull().Build();
    }
}