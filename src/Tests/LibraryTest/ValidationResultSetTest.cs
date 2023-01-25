using Library.Exceptions.Validations;
using Library.Validations;

namespace Library.UnitTest;

[TestClass]
public class ValidationResultSetTest
{
    [TestMethod]
    [ExpectedException(typeof(NullReferenceException))]
    public void _01_BasicTest()
    {
        var arg = new Person("", 0);
        arg = arg.Check().RuleFor(x => !x.Name.IsNullOrEmpty(), () => new NullReferenceException()).ThrowOnFail();
    }

    [TestMethod]
    [ExpectedException(typeof(NullReferenceException))]
    public void _02_IsNullOrEmptyStringTest()
    {
        var arg = new Person("", 0);
        arg = arg.Check().NotNullOrEmpty(x => x.Name).Build().ThrowOnFail();
    }

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void _03_IsBiggerThanIntTest()
    {
        var arg = new Person("", 20);
        arg = arg.Check().NotBiggerThan(x => x.Age, 10).ThrowOnFail();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void _04_IsArgumentNullNullTest()
    {
        Person? arg = null;
        _ = arg.Check().ArgumentNotNull().ThrowOnFail();
    }

    [TestMethod]
    public void _05_IsArgumentNullNotNullTest()
    {
        var arg = new Person("", 20);
        _ = arg.Check().ArgumentNotNull().ThrowOnFail();
    }

    [TestMethod]
    public void _06_FullValidationValidTest()
    {
        var arg = new Person("Ali", 20);
        arg = arg.Check()
                 .ArgumentNotNull()
                 .NotNullOrEmpty(x => x.Name)
                 .NotBiggerThan(x => x.Age, 25)
                 .Build()
                 .ThrowOnFail();
    }

    [TestMethod]
    [ExpectedException(typeof(NullReferenceException))]
    public void _07_FullValidationIsNullOrEmptyTest()
    {
        var arg = new Person("", 20);
        arg = arg.Check()
                 .ArgumentNotNull()
                 .NotNullOrEmpty(x => x.Name)
                 .NotBiggerThan(x => x.Age, 25)
                 .Build()
                 .ThrowOnFail();
    }

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void _08_FullValidationIsNotBiggerThanTest()
    {
        var arg = new Person("Ali", 20);
        arg = arg.Check()
                 .ArgumentNotNull()
                 .NotNullOrEmpty(x => x.Name)
                 .NotBiggerThan(x => x.Age, 10)
                 .Build()
                 .ThrowOnFail();
    }

    [TestMethod]
    public void _09_FullValidationInvalidTest()
    {
        var arg = new Person("", 20);
        var result = arg.Check()
                        .ArgumentNotNull()
                        .NotNullOrEmpty(x => x.Name)
                        .NotBiggerThan(x => x.Age, 10)
                        .Build();
        Assert.AreEqual(2, result.Errors?.Count());
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void _10_FullValidationInvalidTest()
    {
        Person? arg = null;
        var result = arg.Check()
                        .ArgumentNotNull()
                        .NotNullOrEmpty(x => x!.Name)
                        .NotBiggerThan(x => x!.Age, 10)
                        .ThrowOnFail();
    }

    [TestMethod]
    [ExpectedException(typeof(NullReferenceException))]
    public void _11_FullValidationIsNotBiggerThanTest()
    {
        var arg = new Person("", 20);
        var result = arg.Check()
                        .ArgumentNotNull()
                        .NotNullOrEmpty(x => x!.Name)
                        .NotBiggerThan(x => x!.Age, 10)
                        .ThrowOnFail();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void _12_IsArgumentNullNullNotIsLazyTest()
    {
        Person? arg = null;
        _ = arg.Check(true).ArgumentNotNull();
    }
}

file record Person(string Name, int Age);