#nullable disable

using Library;
using Library.Exceptions.Validations;
using Library.Validations;

namespace UnitTests;


[Trait("Category", "Validation Tests")]
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

    [Fact]
    public void ArgumentNullNotNullTest()
        => Check.IfArgumentNotNull(this._string_sample1);

    [Fact]
    public void ArgumentNullNullTest()
        => Assert.Throws<ArgumentNullException>(() => Check.IfArgumentNotNull(this._string_null));

    [Fact]
    public void IfFalseTest()
        => Assert.Throws< ValidationException>(()=> Check.If(false));

    [Fact]
    public void IfHasAnyEmpty()
        => Assert.Throws<ValidationException>(() => Check.IfHasAny(this._array_empty, () => new ValidationException()));

    [Fact]
    public void IfHasAnyNull()
        => Assert.Throws<ValidationException>(() => Check.IfHasAny(this._array_null, () => new ValidationException()));

    [Fact]
    public void IfHasAnyPure()
        => Check.IfHasAny(this._array_sample1_null, () => new ValidationException());

    [Fact]
    public void IfHasAnyTrue()
        => Check.IfHasAny(this._array_sample1_sample, () => new ValidationException());

    [Fact]
    public void IfIs1()
        => Check.IfIs<string>(this._string_sample1);

    [Fact]
    public void IfIs2()
        => Assert.Throws<TypeMismatchValidationException>(() => Check.IfIs<int>(this._string_sample1));

    [Fact]
    public void IfMessageFalseTest()
        => Assert.Throws<ValidationException>(() => Check.If(false, () => this._string_sample1));

    [Fact]
    public void IfMessageTrueTest()
        => Check.If(true, () => this._string_sample1);

    [Fact]
    public void IfNotNullFalse()
        => Assert.Throws<NullValueValidationException>(() => Check.NotNull(null));

    [Fact]
    public void IfNotNullFalseMessage()
        => Assert.Throws<NullValueValidationException>(() => Check.NotNull(null, () => this._string_sample1));

    [Fact]
    public void IfNotNullFalseObject()
        => Assert.Throws<NullValueValidationException>(() => Check.NotNull(this, null, this._string_sample1));

    [Fact]
    public void IfNotNullTrue()
        => Check.NotNull(this._string_sample1);

    [Fact]
    public void IfNotNullTrueMessage()
        => Check.NotNull(this._string_sample1, () => this._string_sample1);

    [Fact]
    public void IfNotNullTrueObject()
        => Check.NotNull(this, this._string_sample1, this._string_sample1);

    [Fact]
    public void IfNotValidNotNullStringTest()
        => Check.NotNull(this._string_sample1, () => new NullValueValidationException());

    [Fact]
    public void IfNotValidNotNullTest()
        => Check.NotValid(this._string_sample1, x => x is not null, () => new NullValueValidationException());

    [Fact]
    public void IfNotValidNullStringTest()
        => Assert.Throws<NullValueValidationException>(() => Check.NotNull(this._string_null, () => new NullValueValidationException()));

    [Fact]
    public void IfNotValidNullTest()
        => Assert.Throws<NullValueValidationException>(() => Check.NotValid(this._string_null, x => x is not null, () => new NullValueValidationException()));

    [Fact]
    public void IfRequiredFalseTest()
        => Assert.Throws<RequiredValidationException>(() => Check.IfRequired(false));

    [Fact]
    public void IfRequiredTrueTest()
        => Check.IfRequired(true);

    [Fact]
    public void IfTrueTest()
        => Check.If(true);

    [Fact]
    public void MinMaxTest()
    {
        var (min, arg) = (0, 5);
        Check.IfArgumentBiggerThan(arg, min);
    }

    [Fact]
    public void MinMaxTest1()
    {
        var (arg, min) = (0, 5);
        Assert.Throws<ArgumentException>(() => Check.IfArgumentBiggerThan(arg, min));
    }

    [Fact]
    public void MustBeArgumentNotNullNotNullTest()
    {
        var result = Check.MustBeArgumentNotNull(this._array_sample1_sample);
        Assert.True(result);
    }

    [Fact]
    public void MustBeArgumentNotNullNullTest()
    {
        var result = Check.MustBeArgumentNotNull(this._string_null);
        Assert.False(result);
    }

    [Fact]
    public void MustBeFalse()
    {
        var result = Check.MustBe(this, false, () => new ValidationException());
        Assert.False(result);
        Assert.IsType<ValidationException>(result.Errors?.ElementAt(0).Error);
    }

    [Fact]
    public void MustBeFalseMessage()
    {
        var result = Check.MustBe(false, () => this._string_sample1);
        Assert.False(result);
    }

    [Fact]
    public void MustBeFalseTest()
    {
        var result = Check.MustBe(false);
        Assert.False(result);
    }

    [Fact]
    public void MustBeGenericFalseTest()
    {
        var result = Check.MustBe(5, false);
        Assert.Equal(5, result);
        Assert.False(result);
    }

    [Fact]
    public void MustBeGenericTrueTest()
    {
        var result = Check.MustBe(5, true);
        Assert.Equal(5, result);
        Assert.True(result);
    }

    [Fact]
    public void MustBeInstanceGetStringNotNullTest()
    {
        var result = Check.MustBeNotNull(this._object_sample1, () => this._string_sample1);
        Assert.True(result!);
    }

    [Fact]
    public void MustBeInstanceGetStringNullTest()
    {
        var result = Check.MustBeNotNull(this, this._string_null);
        Assert.False(result!);
    }

    [Fact]
    public void MustBeInstanceStringNotNullTest()
    {
        var result = Check.MustBeNotNull(this, this._string_sample1);
        Assert.True(result!);
    }

    [Fact]
    public void MustBeInstanceStringNullTest()
    {
        var result = Check.MustBeNotNull(this._object_null, () => this._string_sample1);
        Assert.False(result);
    }

    [Fact]
    public void MustBeNotNullObjectNotNullTest()
    {
        var result = Check.MustBeNotNull(this._object_sample1);
        Assert.True(result!);
    }

    [Fact]
    public void MustBeNotNullObjectNullTest()
    {
        var result = Check.MustBeNotNull(this._object_null);
        Assert.False(result!);
    }

    [Fact]
    public void MustBeNotNullStringNotNullTest()
    {
        var result = Check.MustBeNotNull(this._string_sample1);
        Assert.True(result!);
    }

    [Fact]
    public void MustBeNotNullStringNullTest()
    {
        var result = Check.MustBeNotNull(this._string_null);
        Assert.False(result!);
    }

    [Fact]
    public void MustBeObjectPredicateExceptionFalseTest()
    {
        var result = Check.MustBe(this._string_sample1, _ => false, () => new ValidationException());
        Assert.False(result);
        Assert.IsType<ValidationException>(result.Errors.ElementAt(0).Error);
    }

    [Fact]
    public void MustBeObjectPredicateExceptionTrueTest()
    {
        var result = Check.MustBe(this._string_sample1, _ => true, () => new ValidationException());
        Assert.True(result);
        Assert.Equal(this._string_sample1, result!);
        Assert.Null(result.Status);
    }

    [Fact]
    public void MustBeObjectTrueExceptionTest()
    {
        var result = Check.MustBe(this, true, () => new ValidationException());
        Assert.True(result!);
        Assert.Null(result.Status);
    }

    [Fact]
    public void MustBeOutFalse()
    {
        _ = Check.MustBe(this._string_sample1, () => false, () => new ValidationException(), out var result);
        Assert.False(result);
        Assert.IsType<ValidationException>(result.Errors.First().Error);
    }

    [Fact]
    public void MustBeOutTrue()
    {
        _ = Check.MustBe(this._string_sample1, () => true, () => new ValidationException(), out var result);
        Assert.True(result);
        Assert.Equal(this._string_sample1, result!);
    }

    [Fact]
    public void MustBePredicateExceptionFalseTest()
    {
        var result = Check.MustBe(() => false, () => new ValidationException());
        Assert.False(result);
        Assert.IsType<ValidationException>(result.Errors.First().Error);
    }

    [Fact]
    public void MustBePredicateExceptionTrueTest()
    {
        var result = Check.MustBe(() => true, () => new ValidationException());
        Assert.True(result);
    }

    [Fact]
    public void MustBePredicateFalseMessageTest()
    {
        var result = Check.MustBe(() => false, () => this._string_sample1);
        Assert.False(result);
    }

    [Fact]
    public void MustBePredicateFalseTest()
    {
        var result = Check.MustBe(this, () => false, () => new ValidationException());
        Assert.False(result);
        Assert.IsType<ValidationException>(result.Errors.First().Error);
    }

    [Fact]
    public void MustBePredicateGetExceptionFalseTest()
    {
        var result = Check.MustBe(() => false, () => new ValidationException());
        Assert.False(result);
        Assert.IsType<ValidationException>(result.Errors.First().Error);
    }

    [Fact]
    public void MustBePredicateGetExceptionTrueTest()
    {
        var result = Check.MustBe(() => true, () => new ValidationException());
        Assert.True(result);
    }

    [Fact]
    public void MustBePredicateTrueMessageTest()
    {
        var result = Check.MustBe(() => true, () => this._string_sample1);
        Assert.True(result);
    }

    [Fact]
    public void MustBePredicateTrueTest()
    {
        var result = Check.MustBe(this, () => true, () => new ValidationException());
        Assert.True(result);
    }

    [Fact]
    public void MustBeTrueMessageTest()
    {
        var result = Check.MustBe(true, () => this._string_sample1);
        Assert.True(result);
    }

    [Fact]
    public void MustBeTrueTest()
    {
        var result = Check.MustBe(true);
        Assert.True(result);
    }

    [Fact]
    public void MustNotBeNullInstanceObjectNotNullTest()
    {
        var result = Check.MustBeNotNull(this, this._object_sample1);
        Assert.True(result!);
    }

    [Fact]
    public void MustNotBeNullInstanceObjectNullTest()
    {
        var result = Check.MustBeNotNull(this, this._object_null);
        Assert.False(result!);
    }

    [Fact]
    public void TryMustBeArgumentNotNullNotNullTest()
    {
        _ = Check.TryMustBeArgumentNotNull(this._object_sample1, out var result);
        Assert.True(result);
    }

    [Fact]
    public void TryMustBeArgumentNotNullNullTest()
    {
        _ = Check.TryMustBeArgumentNotNull(this._object_null, out var result);
        Assert.False(result);
    }
}