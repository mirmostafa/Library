using System.Collections;

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
    [ExpectedException(typeof(ValidationException))]
    public void IfFalseTest()
        => Check.If(false);

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void IfHasAnyFalse()
    {
        var array = Array.Empty<string>();
        Check.IfHasAny(array, () => new ValidationException());
    }

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void IfHasAnyNull()
    {
        string[]? array = null;
        Check.IfHasAny(array, () => new ValidationException());
    }

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void IfHasAnyPure()
    {
        IEnumerable array = Array.Empty<string>();
        Check.IfHasAny(array, () => new ValidationException());
    }

    [TestMethod]
    public void IfHasAnyTrue()
    {
        var array = new string[1];
        Check.IfHasAny(array, () => new ValidationException());
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
    [ExpectedException(typeof(ValidationException))]
    public void IfMessageFalseTest()
        => Check.If(false, () => "Hi");

    [TestMethod]
    public void IfMessageTrueTest()
        => Check.If(true, () => "Hi");

    [TestMethod]
    [ExpectedException(typeof(RequiredValidationException))]
    public void IfRequiredFalseTest() => Check.IfRequired(false);

    [TestMethod]
    public void IfRequiredTrueTest() => Check.IfRequired(true);

    [TestMethod]
    public void IfTrueTest() => Check.If(true);

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
    public void MustBeFalseTest()
    {
        var result = Check.MustBe(false);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void MustBeGenericFalseTest()
    {
        var result = Check.MustBe(5, false);
        Assert.AreEqual(5, result);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void MustBeGenericTrueTest()
    {
        var result = Check.MustBe(5, true);
        Assert.AreEqual(5, result);
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void MustBeTrueTest()
    {
        var result = Check.MustBe(true);
        Assert.IsTrue(result);
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