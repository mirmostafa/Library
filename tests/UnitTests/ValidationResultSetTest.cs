using Library.Validations;

namespace UnitTests;

[Trait("Category", "Validation Tests")]
public class ValidationResultSetTest
{
    [Fact]
    public void _01_Basic()
    {
        var arg = new Person("", 0);
        var check = arg.Check().RuleFor(x => !x.Name.IsNullOrEmpty(), () => new NullReferenceException());
        _ = Assert.Throws<NullReferenceException>(check.ThrowOnFail);
    }

    [Fact]
    public void _02_IsNullOrEmpty_String()
    {
        var arg = new Person("", 0);
        var check = arg.Check().NotNullOrEmpty(x => x.Name).Build();
        _ = Assert.Throws<Library.Exceptions.Validations.NullValueValidationException>(() => check.ThrowOnFail());
    }

    [Fact]
    public void _03_IsBiggerThan_Int()
    {
        var arg = new Person("", 700);
        var check = arg.Check().NotBiggerThan(x => x.Age, 120);
        _ = Assert.Throws<Library.Exceptions.Validations.ValidationException>(check.ThrowOnFail);
    }

    [Fact]
    public void _04_IsArgumentNull_Null()
    {
        Person? arg = null;
        var check = arg.Check().ArgumentNotNull();
        _ = Assert.Throws<ArgumentNullException>(check.ThrowOnFail);
    }

    [Fact]
    public void _05_IsArgumentNullNot_Null()
    {
        var arg = new Person("Ali", 0);
        var check = arg.Check().ArgumentNotNull();
        _ = check.ThrowOnFail();
    }

    [Fact]
    public void _06_FullValidation_Valid()
    {
        var arg = new Person("Ali", 0);
        var check = arg.Check()
            .ArgumentNotNull()
            .NotNullOrEmpty(x => x.Name)
            .NotBiggerThan(x => x.Age, 25)
            .Build();
        _ = check.ThrowOnFail();
    }

    [Fact]
    public void _07_FullValidationIsNullOrEmpty()
    {
        var arg = new Person("", 20);
        var check = arg.Check()
            .ArgumentNotNull()
            .NotNullOrEmpty(x => x.Name)
            .NotBiggerThan(x => x.Age, 25)
            .Build();
        _ = Assert.Throws<Library.Exceptions.Validations.NullValueValidationException>(() => check.ThrowOnFail());
    }

    [Fact]
    public void _08_FullValidationIsNotBiggerThan()
    {
        var arg = new Person("Ali", 20);
        var check = arg.Check()
            .ArgumentNotNull()
            .NotNullOrEmpty(x => x.Name)
            .NotBiggerThan(x => x.Age, 10)
            .Build();
        _ = Assert.Throws<Library.Exceptions.Validations.ValidationException>(() => check.ThrowOnFail());
    }

    [Fact]
    public void _09_FullValidation_Invalid()
    {
        var arg = new Person("", 20);
        var result = arg.Check()
            .ArgumentNotNull()
            .NotNullOrEmpty(x => x.Name)
            .NotBiggerThan(x => x.Age, 10)
            .BuildAll();
        Assert.Equal(2, result.Errors?.Count());
    }

    [Fact]
    public void _10_FullValidation_Invalid()
    {
        Person? arg = null;
        var check = arg.Check()
            .ArgumentNotNull()
            .NotNullOrEmpty(x => x!.Name)
            .NotBiggerThan(x => x!.Age, 10);
        _ = Assert.Throws<ArgumentNullException>(check.ThrowOnFail);
    }

    [Fact]
    public void _11_FullValidationIsNotBiggerThan()
    {
        var arg = new Person("", 20);
        var check = arg.Check()
            .ArgumentNotNull()
            .NotNullOrEmpty(x => x!.Name)
            .NotBiggerThan(x => x!.Age, 10);
        _ = Assert.Throws<Library.Exceptions.Validations.NullValueValidationException>(check.ThrowOnFail);
    }

    [Fact]
    public void _12_IsArgumentNull_Null_ThrowOnFail()
    {
        Person? arg = null;
        var check = arg.Check(true);
        _ = Assert.Throws<ArgumentNullException>(() => check.ArgumentNotNull());
    }
}

file record Person(string Name, int Age);