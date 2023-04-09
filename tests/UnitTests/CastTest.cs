using Library.Coding;
using Library.Helpers;
using Library.Validations;

namespace UnitTests;

[Trait("Category", "Code Helpers")]
public sealed class CastTest
{
    [Fact]
    public void AsTest1()
    {
        Student p = new();
        var s = p.Cast().As<Person>();
        Assert.NotNull(s);
    }

    [Fact]
    public void AsTest2()
    {
        Person p = new();
        var s = p.Cast().As<Student>();
        Assert.Null(s);
    }
}

file record Person;

file record Student : Person { }