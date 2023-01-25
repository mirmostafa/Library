#nullable disable

using Library.Exceptions.Validations;
using Library.Validations;

namespace Library.UnitTest;

[TestClass]
public class CheckValidationTest
{
    private readonly Array _array_empty = Array.Empty<string>();
    private readonly Array _array_null = null;
    private readonly Array _array_sample1_null = new string[1];
    private readonly Array _array_sample1_sample = new string[1] { "Hi" };
    private readonly object _object_null = null;
    private readonly object _object_sample1 = new();
    private readonly string _string_null = null;
    private readonly string _string_sample1 = "Hi";

    [TestMethod]
    public void ArgumentNullNotNullTest()
        => Check.IfArgumentNotNull(this._string_sample1);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ArgumentNullNullTest()
        => Check.IfArgumentNotNull(this._string_null);

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void IfFalseTest()
        => Check.If(false);

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void IfHasAnyFalse()
        => Check.IfHasAny(this._array_empty, () => new ValidationException());

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void IfHasAnyNull()
        => Check.IfHasAny(this._array_null, () => new ValidationException());

    [TestMethod]
    public void IfHasAnyPure()
        => Check.IfHasAny(this._array_sample1_null, () => new ValidationException());

    [TestMethod]
    public void IfHasAnyTrue()
        => Check.IfHasAny(this._array_sample1_sample, () => new ValidationException());

    [TestMethod]
    public void IfIs1()
        => Check.IfIs<string>(this._string_sample1);

    [TestMethod]
    [ExpectedException(typeof(TypeMismatchValidationException))]
    public void IfIs2()
        => Check.IfIs<int>(this._string_sample1);

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void IfMessageFalseTest()
        => Check.If(false, () => this._string_sample1);

    [TestMethod]
    public void IfMessageTrueTest()
        => Check.If(true, () => this._string_sample1);

    [TestMethod]
    [ExpectedException(typeof(NullValueValidationException))]
    public void IfNotNullFalse()
        => Check.IfNotNull(null);

    [TestMethod]
    [ExpectedException(typeof(NullValueValidationException))]
    public void IfNotNullFalseMessage()
        => Check.IfNotNull(null, () => this._string_sample1);

    [TestMethod]
    [ExpectedException(typeof(NullValueValidationException))]
    public void IfNotNullFalseObject()
        => Check.IfNotNull(this, null, this._string_sample1);

    [TestMethod]
    public void IfNotNullTrue()
        => Check.IfNotNull(this._string_sample1);

    [TestMethod]
    public void IfNotNullTrueMessage()
        => Check.IfNotNull(this._string_sample1, () => this._string_sample1);

    [TestMethod]
    public void IfNotNullTrueObject()
        => Check.IfNotNull(this, this._string_sample1, this._string_sample1);

    [TestMethod]
    public void IfNotValidNotNullStringTest()
        => Check.IfNotNull(this._string_sample1, () => new NullValueValidationException());

    [TestMethod]
    public void IfNotValidNotNullTest()
        => Check.IfNotValid(this._string_sample1, x => x is null, () => new NullValueValidationException());

    [TestMethod]
    [ExpectedException(typeof(NullValueValidationException))]
    public void IfNotValidNullStringTest()
        => Check.IfNotNull(this._string_null, () => new NullValueValidationException());

    [TestMethod]
    [ExpectedException(typeof(NullValueValidationException))]
    public void IfNotValidNullTest()
        => Check.IfNotValid(this._string_null, x => x is null, () => new NullValueValidationException());

    [TestMethod]
    [ExpectedException(typeof(RequiredValidationException))]
    public void IfRequiredFalseTest()
        => Check.IfRequired(false);

    [TestMethod]
    public void IfRequiredTrueTest()
        => Check.IfRequired(true);

    [TestMethod]
    public void IfTrueTest()
        => Check.If(true);

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
    public void MustBeArgumentNotNullNotNullTest()
    {
        var result = Check.MustBeArgumentNotNull(this._array_sample1_sample);
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void MustBeArgumentNotNullNullTest()
    {
        var result = Check.MustBeArgumentNotNull(this._string_null);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void MustBeFalse()
    {
        var result = Check.MustBe(this, false, () => new ValidationException());
        Assert.IsFalse(result);
        Assert.IsInstanceOfType(result.Errors?.ElementAt(0).Error, typeof(ValidationException));
    }

    [TestMethod]
    public void MustBeFalseMessage()
    {
        var result = Check.MustBe(false, () => this._string_sample1);
        Assert.IsFalse(result);
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
    public void MustBeInstanceGetStringNotNullTest()
    {
        var result = Check.MustBeNotNull(this._object_sample1, () => this._string_sample1);
        Assert.IsTrue(result!);
    }

    [TestMethod]
    public void MustBeInstanceGetStringNullTest()
    {
        var result = Check.MustBeNotNull(this, this._string_null);
        Assert.IsFalse(result!);
    }

    [TestMethod]
    public void MustBeInstanceStringNotNullTest()
    {
        var result = Check.MustBeNotNull(this, this._string_sample1);
        Assert.IsTrue(result!);
    }

    [TestMethod]
    public void MustBeInstanceStringNullTest()
    {
        var result = Check.MustBeNotNull(this._object_null, () => this._string_sample1);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void MustBeNotNullObjectNotNullTest()
    {
        var result = Check.MustBeNotNull(this._object_sample1);
        Assert.IsTrue(result!);
    }

    [TestMethod]
    public void MustBeNotNullObjectNullTest()
    {
        var result = Check.MustBeNotNull(this._object_null);
        Assert.IsFalse(result!);
    }

    [TestMethod]
    public void MustBeNotNullStringNotNullTest()
    {
        var result = Check.MustBeNotNull(this._string_sample1);
        Assert.IsTrue(result!);
    }

    [TestMethod]
    public void MustBeNotNullStringNullTest()
    {
        var result = Check.MustBeNotNull(this._string_null);
        Assert.IsFalse(result!);
    }

    [TestMethod]
    public void MustBeObjectPredicateExceptionFalseTest()
    {
        var result = Check.MustBe(this._string_sample1, _ => false, () => new ValidationException());
        Assert.IsFalse(result);
        Assert.IsInstanceOfType(result.Errors.ElementAt(0).Error, typeof(ValidationException));
    }

    [TestMethod]
    public void MustBeObjectPredicateExceptionTrueTest()
    {
        var result = Check.MustBe(this._string_sample1, _ => true, () => new ValidationException());
        Assert.IsTrue(result);
        Assert.AreEqual(this._string_sample1, result!);
        Assert.IsNull(result.Status);
    }

    [TestMethod]
    public void MustBeObjectTrueExceptionTest()
    {
        var result = Check.MustBe(this, true, () => new ValidationException());
        Assert.IsTrue(result!);
        Assert.IsNull(result.Status);
    }

    [TestMethod]
    public void MustBeOutFalse()
    {
        _ = Check.MustBe(this._string_sample1, () => false, () => new ValidationException(), out var result);
        Assert.IsFalse(result);
        Assert.IsInstanceOfType(result.Errors.First().Error, typeof(ValidationException));
    }

    [TestMethod]
    public void MustBeOutTrue()
    {
        _ = Check.MustBe(this._string_sample1, () => true, () => new ValidationException(), out var result);
        Assert.IsTrue(result);
        Assert.AreEqual(this._string_sample1, result!);
    }

    [TestMethod]
    public void MustBePredicateExceptionFalseTest()
    {
        var result = Check.MustBe(() => false, () => new ValidationException());
        Assert.IsFalse(result);
        Assert.IsInstanceOfType(result.Errors.First().Error, typeof(ValidationException));
    }

    [TestMethod]
    public void MustBePredicateExceptionTrueTest()
    {
        var result = Check.MustBe(() => true, () => new ValidationException());
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void MustBePredicateFalseMessageTest()
    {
        var result = Check.MustBe(() => false, () => this._string_sample1);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void MustBePredicateFalseTest()
    {
        var result = Check.MustBe(this, () => false, () => new ValidationException());
        Assert.IsFalse(result);
        Assert.IsInstanceOfType(result.Errors.First().Error, typeof(ValidationException));
    }

    [TestMethod]
    public void MustBePredicateGetExceptionFalseTest()
    {
        var result = Check.MustBe(() => false, () => new ValidationException());
        Assert.IsFalse(result);
        Assert.IsInstanceOfType(result.Errors.First().Error, typeof(ValidationException));
    }

    [TestMethod]
    public void MustBePredicateGetExceptionTrueTest()
    {
        var result = Check.MustBe(() => true, () => new ValidationException());
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void MustBePredicateTrueMessageTest()
    {
        var result = Check.MustBe(() => true, () => this._string_sample1);
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void MustBePredicateTrueTest()
    {
        var result = Check.MustBe(this, () => true, () => new ValidationException());
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void MustBeTrueMessageTest()
    {
        var result = Check.MustBe(true, () => this._string_sample1);
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void MustBeTrueTest()
    {
        var result = Check.MustBe(true);
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void MustNotBeNullInstanceObjectNotNullTest()
    {
        var result = Check.MustBeNotNull(this, this._object_sample1);
        Assert.IsTrue(result!);
    }

    [TestMethod]
    public void MustNotBeNullInstanceObjectNullTest()
    {
        var result = Check.MustBeNotNull(this, this._object_null);
        Assert.IsFalse(result!);
    }

    [TestMethod]
    public void TryMustBeArgumentNotNullNotNullTest()
    {
        _ = Check.TryMustBeArgumentNotNull(this._object_sample1, out var result);
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void TryMustBeArgumentNotNullNullTest()
    {
        _ = Check.TryMustBeArgumentNotNull(this._object_null, out var result);
        Assert.IsFalse(result);
    }
}