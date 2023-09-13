using Library.Exceptions.Validations;
using Library.Validations;

namespace UnitTests;

[Trait("Category", "Validation Tests")]
public sealed class ValidationResultSetTest
{
    [Fact]
    public void _01_Basic()
    {
        var arg = new Person("", 0);
        _ = Assert.Throws<NullReferenceException>(() => arg.Check(CheckBehavior.ThrowOnFail).RuleFor(x => !x.Name.IsNullOrEmpty(), () => new NullReferenceException()));
    }

    [Fact]
    public void _02_IsNullOrEmpty_String()
    {
        var arg = new Person("", 0);
        _ = Assert.Throws<NullValueValidationException>(() => arg.Check(CheckBehavior.ThrowOnFail).NotNullOrEmpty(x => x.Name));
    }

    [Fact]
    public void _04_IsArgumentNull_Null()
    {
        Person? arg = null;
        _ = Assert.Throws<ArgumentNullException>(() => arg.Check(CheckBehavior.ThrowOnFail).ArgumentNotNull());
    }

    [Fact]
    public void _05_IsArgumentNullNot_Null()
    {
        var arg = new Person("Ali", 0);
        _ = arg.Check().ArgumentNotNull();
    }

    [Fact]
    public void _06_FullValidation_Valid()
    {
        var arg = new Person("Ali", 0);
        arg = arg.Check()
            .ArgumentNotNull()
            .NotNullOrEmpty(x => x.Name)
            //.NotBiggerThan(x => x.Age, 25)
            .ThrowOnFail();
    }

    [Fact]
    public void _07_Validation_ReturnFirstFailure_ShouldPass()
    {
        var arg = new Person("Ali", 20);
        var check = arg.Check(CheckBehavior.ReturnFirstFailure)
            .ArgumentNotNull()
            .NotNullOrEmpty(x => x.Name)
            //.NotBiggerThan(x => x.Age, 20)
            .Build();
        arg = check;
        Assert.True(check);
    }

    [Fact]
    public void _07_Validation_ReturnFirstFailure_ShouldThrowError()
    {
        var arg = new Person("", 20);
        var check = arg.Check(CheckBehavior.ReturnFirstFailure)
            .ArgumentNotNull()
            .NotNullOrEmpty(x => x.Name)
            //.NotBiggerThan(x => x.Age, 25)
            .Build();
        _ = Assert.Throws<NullValueValidationException>(() => check.ThrowOnFail());
        arg = check;
    }

    [Fact]
    public void _08_FullValidationIsNotBiggerThan()
    {
        var arg = new Person("", 20);
        var check = arg.Check(CheckBehavior.ReturnFirstFailure)
            .ArgumentNotNull()
            .NotNullOrEmpty(x => x.Name)
            .Build();
        _ = Assert.Throws<NullValueValidationException>(() => check.ThrowOnFail());
    }

    [Fact]
    public void _09_FullValidation_Invalid()
    {
        var arg = new Person("", 20);
        var result = arg.Check(CheckBehavior.GatherAll)
            .ArgumentNotNull()
            .NotNullOrEmpty(x => x.Name)
            .Build();
        Assert.Equal(1, result.Errors?.Count());
    }

    [Fact]
    public void _10_FullValidation_Invalid()
    {
        Person? arg = null;
        var check = arg.Check(CheckBehavior.ReturnFirstFailure)
            .ArgumentNotNull()
            .NotNullOrEmpty(x => x!.Name)
            //.NotBiggerThan(x => x!.Age, 10)
            ;
        _ = Assert.Throws<ArgumentNullException>(check.ThrowOnFail);
    }

    [Fact]
    public void _11_FullValidationIsNotBiggerThan()
    {
        var arg = new Person("", 20);
        var check = arg.Check(CheckBehavior.ReturnFirstFailure)
            .ArgumentNotNull()
            .NotNullOrEmpty(x => x!.Name)
            //.NotBiggerThan(x => x!.Age, 10)
            ;
        _ = Assert.Throws<NullValueValidationException>(check.ThrowOnFail);
    }

    [Fact]
    public void _12_IsArgumentNull_Null_ThrowOnFail()
    {
        Person? arg = null;
        var check = arg.Check(CheckBehavior.ThrowOnFail);
        _ = Assert.Throws<ArgumentNullException>(() => check.ArgumentNotNull());
    }

    [Fact]
    public void _13_NotNull_Null_ThrowOnFail()
    {
        Person? arg = null;
        _ = Assert.Throws<NullValueValidationException>(() => arg.Check(CheckBehavior.ThrowOnFail).NotNull());
    }
}

file record Person(string Name, int Age);