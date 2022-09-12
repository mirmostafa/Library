using Library.Exceptions.Validations;
using Library.Validations;

namespace UnitTest;

[TestClass]
public class CheckValidationTest
{
    [TestMethod]
    public void ArgumentNullTest()
    {
        var lname = "Mirmostafa";
        Check.IfArgumentNotNull(lname);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ArgumentNullTest2()
    {
        string? lname = null;
        Check.IfArgumentNotNull(lname);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ArgumentNullTest3()
    {
        var lname = string.Empty;
        Check.IfArgumentNotNull(lname);
    }

    [TestMethod]
    public void FluentCheckTest1()
    {
        object? o = new();
        string? s = null;
        _ = o.MustBe().ArgumentNotNull().ThrowOnFail();
        _ = s.MustBe().NotNull().ThrowOnFail();
    }

    [TestMethod]
    public void IfIs1()
    {
        var lname = "Mirmostafa";
        Check.IfIs<string>(lname);
    }

    [TestMethod]
    [ExpectedException(typeof(TypeMismatchValidationException))]
    public void IfIs2()
    {
        var lname = "Mirmostafa";
        Check.IfIs<int>(lname);
    }

    [TestMethod]
    public void MinMaxTest()
    {
        var (min, arg) = (0, 5);
        Check.IfArgumentBiggerThan(arg, min);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void MinMaxTest1()
    {
        var (arg, min) = (0, 5);
        Check.IfArgumentBiggerThan(arg, min);
    }

    [TestMethod]
    public void NotNullTest1()
    {
        var lname = "Mirmostafa";
        Check.IfNotNull(lname, () => new NullValueValidationException());
    }

    [TestMethod]
    [ExpectedException(typeof(NullValueValidationException))]
    public void NotNullTest2()
    {
        string? lname = null;
        Check.IfNotNull(lname, () => new NullValueValidationException());
    }

    [TestMethod]
    public void NotValidTest1()
    {
        var lname = "Mirmostafa";
        Check.IfNotValid(lname, x => x is null, () => new NullValueValidationException());
    }

    [TestMethod]
    [ExpectedException(typeof(NullValueValidationException))]
    public void NotValidTest2()
    {
        string? lname = null;
        Check.IfNotValid(lname, x => x is null, () => new NullValueValidationException());
    }
}