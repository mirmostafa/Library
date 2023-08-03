#nullable disable

using Library.Exceptions.Validations;
using Library.Validations;

namespace UnitTests;

[Trait("Category", "Validation Tests")]
public sealed class CheckValidationTest
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
        => Check.MustBeArgumentNotNull(this._string_sample1);

    [Fact]
    public void ArgumentNullNullTest()
        => Assert.Throws<ArgumentNullException>(() => Check.MustBeArgumentNotNull(this._string_null));

    [Fact]
    public void IfFalseTest()
        => Assert.Throws<ValidationException>(() => Check.MustBe(false));

    [Fact]
    public void IfHasAnyEmpty()
        => Assert.Throws<ValidationException>(() => Check.MustHaveAny(this._array_empty));

    [Fact]
    public void IfHasAnyNull()
        => Assert.Throws<ValidationException>(() => Check.MustHaveAny(this._array_null));

    [Fact]
    public void IfHasAnyPure()
        => Check.MustHaveAny(this._array_sample1_null);

    [Fact]
    public void IfHasAnyTrue()
        => Check.MustHaveAny(this._array_sample1_sample);

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
        => Assert.Throws<NullValueValidationException>(() => Check.NotNull(null, this._string_sample1));

    [Fact]
    public void IfNotNullTrue()
        => Check.NotNull(this._string_sample1);

    [Fact]
    public void IfNotNullTrueMessage()
        => Check.NotNull(this._string_sample1, () => this._string_sample1);

    [Fact]
    public void IfNotNullTrueObject()
        => Check.NotNull(this._string_sample1, this._string_sample1);

    [Fact]
    public void IfNotValidNotNullStringTest()
        => Check.NotNull(this._string_sample1, () => new NullValueValidationException());

    [Fact]
    public void IfNotValidNullStringTest()
        => Assert.Throws<NullValueValidationException>(() => Check.NotNull(this._string_null, () => new NullValueValidationException()));

    [Fact]
    public void IfTrueTest()
        => Check.MustBe(true);

    [Fact]
    public void MustBeArgumentNotNullNullTest()
    {
        var result = Check.IfArgumentIsNull(this._string_null);
        Assert.False(result);
    }

    [Fact]
    public void MustBeFalse()
    {
        var result = Check.If(false, () => new ValidationException());
        Assert.False(result);
        _ = Assert.IsType<ValidationException>(result.Status);
    }

    [Fact]
    public void MustBeFalseMessage()
    {
        var result = Check.If(false, () => this._string_sample1);
        Assert.False(result);
    }

    [Fact]
    public void MustBeFalseTest()
    {
        var result = Check.If(false);
        Assert.False(result);
    }
}