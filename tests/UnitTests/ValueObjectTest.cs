using System.Diagnostics.CodeAnalysis;

using Library.Data;

using Addess = Library.Data.ValueObject<string>;
using FullName = (UnitTests.Name FirstName, UnitTests.Name LastName);


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
        var p = new Person(name, age)
        {
            Address = "Here",
            FullName = ("Mohammad", "Kiani")
        };
        Assert.True(p.Age == 5);
        Assert.True(p.FirstName == "Ali");
    }
}


sealed file class Person(Name firstName, Age age, Addess? address = null)
{
    public Addess Address { get; set; } = address ?? new(string.Empty);

    public FullName FullName { get; set; }

    public Age Age { get; } = age;
    public Name FirstName { get; } = firstName;
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