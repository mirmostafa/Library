using Library.Validations;

namespace UnitTests;

[Trait("Category", "Validation Tests")]
public class ValidationTest
{
    [Fact(Skip = "Use `ValidationResultSet<TValue>` instead.")]
    [Obsolete("Use `ValidationResultSet<TValue>` instead.", true)]
    public void _01_ValidationResult_BeNotNullNotNullTest()
    {
        var arg = "Test";

        _ = arg.If().NotNull().ThrowOnFail();
    }

    [Fact(Skip = "Use `ValidationResultSet<TValue>` instead.")]
    [Obsolete("Use `ValidationResultSet<TValue>` instead.", true)]
    public void _02_ValidationCheck_BeNotNullNullTest()
    {
        string? arg = null;

        _ = arg.Should().BeNotNull();
    }

    [Fact(Skip = "Use `ValidationResultSet<TValue>` instead.")]
    [Obsolete("Use `ValidationResultSet<TValue>` instead.", true)]
    public void _03_ValidationResult_ArgumentBeNotNullNotNullTest()
    {
        var arg = "Test";

        _ = arg.If().ArgumentIsNotNull().ThrowOnFail();
    }

    [Fact(Skip = "Use `ValidationResultSet<TValue>` instead.")]
    [Obsolete("Use `ValidationResultSet<TValue>` instead.", true)]
    public void _04_ValidationCheck_ArgumentBeNotNullNullTest()
    {
        string? arg = null;

        _ = arg.If().ArgumentIsNotNull().Build();
    }
}