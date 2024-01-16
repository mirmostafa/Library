using Library.Exceptions.Validations;
using Library.Validations;

namespace UnitTests;

[Trait("Category", nameof(Library.Validations))]
[Trait("Category", nameof(ValidationResultSet<string>))]
public sealed class ValidationResultSetTest
{
    [Fact]
    public void Basic()
    {
        var arg = new Person("", 0);
        _ = Assert.Throws<NullReferenceException>(() => arg.Check(CheckBehavior.ThrowOnFail).RuleFor(x => !x.FirstName.IsNullOrEmpty(), () => new NullReferenceException()));
    }

    [Fact]
    public void FullValidationGatherAllInvalid()
    {
        var arg = new Person("", 20);
        var result = arg.Check(CheckBehavior.GatherAll)
            .ArgumentNotNull()
            .NotNullOrEmpty(x => x.FirstName)
            .Build();
        _ = Assert.Single(result.Errors);
    }

    [Fact]
    public void FullValidationInvalid()
    {
        var arg = new Person("", 20);
        var check = arg.Check(CheckBehavior.ReturnFirstFailure)
            .ArgumentNotNull()
            .NotNullOrEmpty(x => x!.FirstName)
            //.NotBiggerThan(x => x!.Age, 10)
            ;
        _ = Assert.Throws<NullValueValidationException>(check.ThrowOnFail);
    }

    [Fact]
    public void FullValidationIsNotBiggerThan()
    {
        var arg = new Person("", 20);
        var check = arg.Check(CheckBehavior.ReturnFirstFailure)
            .ArgumentNotNull()
            .NotNullOrEmpty(x => x.FirstName)
            .Build();
        _ = Assert.Throws<NullValueValidationException>(() => check.ThrowOnFail());
    }

    [Fact]
    public void FullValidationReturnFirstFailureInvalid()
    {
        Person? arg = null;
        var check = arg.Check(CheckBehavior.ReturnFirstFailure)
            .ArgumentNotNull()
            .NotNullOrEmpty(x => x!.FirstName)
            //.NotBiggerThan(x => x!.Age, 10)
            ;
        _ = Assert.Throws<ArgumentNullException>(check.ThrowOnFail);
    }

    [Fact]
    public void FullValidationValid()
    {
        var arg = new Person("Ali", 0);
        arg = arg.Check()
            .ArgumentNotNull()
            .NotNullOrEmpty(x => x.FirstName)
            //.NotBiggerThan(x => x.Age, 25)
            .ThrowOnFail();
    }

    [Fact]
    public void IsArgumentNullNotNull()
    {
        var arg = new Person("Ali", 0);
        _ = arg.Check().ArgumentNotNull();
    }

    [Fact]
    public void IsArgumentNullNull()
    {
        Person? arg = null;
        _ = Assert.Throws<ArgumentNullException>(() => arg.Check(CheckBehavior.ThrowOnFail).ArgumentNotNull());
    }

    [Fact]
    public void IsArgumentNullNullThrowOnFail()
    {
        Person? arg = null;
        var check = arg.Check(CheckBehavior.ThrowOnFail);
        _ = Assert.Throws<ArgumentNullException>(() => check.ArgumentNotNull());
    }

    [Fact]
    public void IsNullOrEmptyString()
    {
        var arg = new Person("", 0);
        _ = Assert.Throws<NullValueValidationException>(() => arg.Check(CheckBehavior.ThrowOnFail).NotNullOrEmpty(x => x.FirstName));
    }

    [Fact]
    public void NotNullNullThrowOnFail()
    {
        Person? arg = null;
        _ = Assert.Throws<NullValueValidationException>(() => arg.Check(CheckBehavior.ThrowOnFail).NotNull());
    }

    [Fact]
    public void ValidationReturnFirstFailureShouldPass()
    {
        var arg = new Person("Ali", 20);
        var check = arg.Check(CheckBehavior.ReturnFirstFailure)
            .ArgumentNotNull()
            .NotNullOrEmpty(x => x.FirstName)
            //.NotBiggerThan(x => x.Age, 20)
            .Build();
        arg = check;
        Assert.True(check.IsSucceed);
    }

    [Fact]
    public void ValidationReturnFirstFailureShouldThrowError()
    {
        var arg = new Person("", 20);
        var check = arg.Check(CheckBehavior.ReturnFirstFailure)
            .ArgumentNotNull()
            .NotNullOrEmpty(x => x.FirstName)
            //.NotBiggerThan(x => x.Age, 25)
            .Build();
        _ = Assert.Throws<NullValueValidationException>(() => check.ThrowOnFail());
        arg = check;
    }
}

file record Person(string FirstName, int Age);