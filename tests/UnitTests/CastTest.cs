namespace UnitTests;

[Trait("Category", "Helpers")]
[Trait("Category", "Code Helpers")]
public class CastTest
{
    [Fact]
    public void AsTest1()
    {
        Student p = new();
        var s = p.As<Person>();
        Assert.NotNull(s);
    }

    [Fact]
    public void AsTest2()
    {
        Person p = new();
        var s = p.As<Student>();
        Assert.Null(s);
    }
}

file class Person
{ }

file class Student : Person
{ }