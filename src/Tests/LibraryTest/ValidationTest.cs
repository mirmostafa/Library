using Library.Exceptions.Validations;
using Library.Validations;

namespace Library.UnitTest;

[TestClass]
public class ValidationTest
{
    [TestMethod]
    [Obsolete("Use `ValidationResultSet<TValue>` instead.", true)]
    public void _01_ValidationResult_BeNotNullNotNullTest()
    {
        var arg = "Test";

        _ = arg.If().NotNull().ThrowOnFail();
    }

    [TestMethod]
    [ExpectedException(typeof(NullValueValidationException))]
    [Obsolete("Use `ValidationResultSet<TValue>` instead.", true)]
    public void _02_ValidationCheck_BeNotNullNullTest()
    {
        string? arg = null;

        _ = arg.Should().BeNotNull();
    }

    [TestMethod]
    [Obsolete("Use `ValidationResultSet<TValue>` instead.", true)]
    public void _03_ValidationResult_ArgumentBeNotNullNotNullTest()
    {
        var arg = "Test";

        _ = arg.If().ArgumentIsNotNull().ThrowOnFail();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    [Obsolete("Use `ValidationResultSet<TValue>` instead.", true)]
    public void _04_ValidationCheck_ArgumentBeNotNullNullTest()
    {
        string? arg = null;

        _ = arg.If().ArgumentIsNotNull().Build();
    }
}