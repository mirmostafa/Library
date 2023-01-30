using Library.Validations;

using UnitTests.Models;

namespace UnitTests;

[Trait("Category", "Validation Tests")]
public class ValidationResultSetTest
{
    [Fact]
    public void _01_BasicTest()
    {
        var arg = new Person("", 0);
        var check = arg.Check().RuleFor(x => !x.Name.IsNullOrEmpty(), () => new NullReferenceException());
        _ = Assert.Throws<System.NullReferenceException>(check.ThrowOnFail);
    }

    [Fact]
    public void _02_IsNullOrEmptyStringTest()
    {
        var arg = new Person("", 0);
        var check = arg.Check().NotNullOrEmpty(x => x.Name).Build();
        _ = Assert.Throws<Library.Exceptions.Validations.NullValueValidationException>(() => check.ThrowOnFail());
    }

    [Fact]
    public void _03_IsBiggerThanIntTest()
    {
        var arg = new Person("", 20);
        var check = arg.Check().NotBiggerThan(x => x.Age, 10);
        _ = Assert.Throws<Library.Exceptions.Validations.ValidationException>(check.ThrowOnFail);
    }

    [Fact]
    public void _04_IsArgumentNullNullTest()
    {
        PersonClass? arg = null;
        var check = arg.Check().ArgumentNotNull();
        _ = Assert.Throws<System.ArgumentNullException>(check.ThrowOnFail);
    }

    [Fact]
    public void _05_IsArgumentNullNotNullTest()
    {
        var arg = new Person("", 20);
        var check = arg.Check().ArgumentNotNull();
        check.ThrowOnFail();
    }

    [Fact]
    public void _06_FullValidationValidTest()
    {
        var arg = new Person("Ali", 20);
        var check = arg.Check()
                         .ArgumentNotNull()
                         .NotNullOrEmpty(x => x.Name)
                         .NotBiggerThan(x => x.Age, 25)
                         .Build();
        check.ThrowOnFail();
    }

    [Fact]
    public void _07_FullValidationIsNullOrEmptyTest()
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
    public void _08_FullValidationIsNotBiggerThanTest()
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
    public void _09_FullValidationInvalidTest()
    {
        var arg = new Person("", 20);
        var result = arg.Check()
                        .ArgumentNotNull()
                        .NotNullOrEmpty(x => x.Name)
                        .NotBiggerThan(x => x.Age, 10)
                        .Build();
        Assert.Equal(2, result.Errors?.Count());
    }

    [Fact]
    public void _10_FullValidationInvalidTest()
    {
        PersonClass? arg = null;
        var check = arg.Check()
                                .ArgumentNotNull()
                                .NotNullOrEmpty(x => x!.Name)
                                .NotBiggerThan(x => x!.Age, 10);
        Assert.Throws<System.ArgumentNullException>(() => check.ThrowOnFail());
    }

    [Fact]
    public void _11_FullValidationIsNotBiggerThanTest()
    {
        var arg = new Person("", 20);
        var check = arg.Check()
                                .ArgumentNotNull()
                                .NotNullOrEmpty(x => x!.Name)
                                .NotBiggerThan(x => x!.Age, 10);
        _ = Assert.Throws<Library.Exceptions.Validations.NullValueValidationException>(check.ThrowOnFail);
    }

    [Fact]
    public void _12_IsArgumentNullNullNotIsLazyTest()
    {
        PersonClass? arg = null;
        var check = arg.Check(true);
        _ = Assert.Throws<System.ArgumentNullException>(() => check.ArgumentNotNull());
    }
}

file record Person(string Name, int Age);