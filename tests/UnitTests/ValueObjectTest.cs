using System.Diagnostics.CodeAnalysis;

using Library.Data;

namespace UnitTests;

[Trait("Category", nameof(Library.Data))]
[Trait("Category", nameof(ValueObject<string>))]
public sealed class ValueObjectTest
{
    [Fact]
    public void StraightForwardTest()
    {
        Name name = "Ali";
        Age age = 5;
        var p = new Person(name, age);
        Assert.True(p.Age == 5);
        Assert.True(p.Name == "Ali");
    }
}

file sealed class Person(Name name, Age age, ValueObject<string>? address = null)
{
    public ValueObject<string> Address { get; set; } = address ?? new(string.Empty);
    public Age Age { get; } = age;
    public Name Name { get; } = name;
}

sealed file record Name(string value) : ValueObjectBase<string, Name>(value)
{
    [return: NotNullIfNotNull(nameof(o.value))]
    public static implicit operator string(in Name o)
        => o == default ? string.Empty : o.value ?? string.Empty;

    [return: NotNull]
    public static implicit operator Name(in string o)
        => new(o);
}
sealed file record Age(int? value) : ValueObjectBase<int?, Age>(value)
{
    [return: NotNullIfNotNull(nameof(o.value))]
    public static implicit operator int?(in Age o)
        => o == default ? default : o.value;

    [return: NotNull]
    public static implicit operator Age(in int? o)
        => new(o);
}