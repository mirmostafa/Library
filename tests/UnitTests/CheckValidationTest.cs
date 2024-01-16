#nullable disable

using Library.Exceptions.Validations;
using Library.Validations;

namespace UnitTests;

[Trait("Category", nameof(Library.Validations))]
[Trait("Category", nameof(Check))]
public sealed class CheckValidationTest
{
    private readonly Array _array_empty = Array.Empty<string>();
    private readonly Array _array_null = null;
    private readonly Array _array_sample1_null = new string[1];
    private readonly Array _array_sample1_sample = new string[1] { "Hi" };
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
        => Assert.Throws<NoItemValidationException>(() => Check.MustHaveAny(this._array_empty));

    [Fact]
    public void IfHasAnyNull()
        => Assert.Throws<NoItemValidationException>(() => Check.MustHaveAny(this._array_null));

    [Fact]
    public void IfHasAnyPure()
        => Check.MustHaveAny(this._array_sample1_null);

    [Fact]
    public void IfHasAnyTrue()
        => Check.MustHaveAny(this._array_sample1_sample);

    [Fact]
    public void IfMessageFalseTest()
        => Assert.Throws<ValidationException>(() => Check.MustBe(false, () => this._string_sample1));

    [Fact]
    public void IfMessageTrueTest()
        => Check.If(true, () => this._string_sample1);

    [Fact]
    public void IfNotNullFalse()
        => Assert.Throws<NullValueValidationException>(() => Check.MustBeNotNull(null));

    [Fact]
    public void IfNotNullFalseMessage()
        => Assert.Throws<NullValueValidationException>(() => Check.MustBeNotNull(null, () => this._string_sample1));

    [Fact]
    public void IfNotNullFalseObject()
        => Assert.Throws<NullValueValidationException>(() => Check.MustBeNotNull(null, this._string_sample1));

    [Fact]
    public void IfNotNullTrue()
        => Check.MustBeNotNull(this._string_sample1);

    [Fact]
    public void IfNotNullTrueMessage()
        => Check.MustBeNotNull(this._string_sample1, () => this._string_sample1);

    [Fact]
    public void MustBeNotNull_TrueObject()
        => Check.MustBeNotNull(this._string_sample1, nameof(this._string_sample1));

    [Fact]
    public void MustBeNotNull_NotNullStringTest()
        => Check.MustBeNotNull(this._string_sample1, () => new NullValueValidationException());

    [Fact]
    public void MustBeNotNull_NullStringTest()
        => Assert.Throws<NullValueValidationException>(() => 
        Check.MustBeNotNull(this._string_null, () => new NullValueValidationException()));

    [Fact]
    public void IfTrueTest()
        => Check.MustBe(true);

    [Fact]
    public void IfArgumentIsNotNull_NullTest()
    {
        var result = Check.IfArgumentIsNull(this._string_null);

        Assert.False(result.IsSucceed);
    }

    [Fact]
    public void MustBeFalse()
    {
        string a = null;
        var result = Check.If(a is null, () => new ValidationException());
        Assert.False(result.IsSucceed);
        _ = Assert.IsType<ValidationException>(result.Status);
    }

    [Fact]
    public void MustBeFalseMessage()
    {
        var a = 5;
        var result = Check.If(a == 5, () => this._string_sample1);
        Assert.False(result.IsSucceed);
    }

    [Fact]
    public void MustBeFalseTest()
    {
        var notOk = true;
        var result = Check.If(notOk);
        Assert.False(result.IsSucceed);
    }
}