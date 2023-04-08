using Library.Exceptions.Validations;
using Library.Results;

namespace UnitTests;


[Trait("Category", "Helpers")]
[Trait("Category", "Validation Tests")]
public sealed class ValidationHelperTest
{
    [Fact]
    public void _01_ValidateAllTrueTest()
    {
        Assert.Throws<NoItemValidationException>(() => Check("Starts and End"));
    }

    [Fact]
    public void _02_ValidateAllFalseTest()
    {
        Assert.Throws<NoItemValidationException>(() => Check("Starts and Ends"));
    }

    private static Result<string> Check(string INPUT)
    {
        Result<string> startsWithStart(string x)
            => x.StartsWith("Start")
                ? Result<string>.CreateSuccess(INPUT)
                : Result<string>.CreateFailure("Not started with 'Start'", 2, INPUT)!;
        Result<string> endsWithStart(string x)
            => x.EndsWith("End")
                ? Result<string>.CreateSuccess(INPUT)
                : Result<string>.CreateFailure("Not ended with 'End'", 2, INPUT)!;
        return ValidationHelper.CheckAll(INPUT, startsWithStart, endsWithStart);
    }
}