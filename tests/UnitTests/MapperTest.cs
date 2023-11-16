using System;

using Library.Data.Markers;
using Library.Mapping;

using UnitTests.Models;

using Xunit;

namespace UnitTests;


[Trait("Category", nameof(Library.Mapping))]
public sealed class MapperTest
{
    [Fact]
    public void RecordMappingTest()
    {
        var mapper = new Mapper();
        var student = new StudentRecord("Ali", 5, "Math");
        var person = mapper.Map<PersonClass>(student);
        Assert.NotNull(student);
        Assert.Equal(person.Name, student.Name);
        Assert.Equal(person.Age, student.Age);
    }

    [Fact]
    public void ReverseTest()
    {
        var mapper = new Mapper();
        var person = new PersonClass { Name = "Ali", Age = 10 };
        var student = mapper.Map<StudentClass>(person).ForMember(x => x.Major = "Math");
        Assert.NotNull(student);
        Assert.Equal(person.Name, student.Name);
        Assert.Equal(person.Age, student.Age);
        Assert.Equal("Math", student.Major);
    }

    [Fact]
    public void StraightForwardTest()
    {
        var mapper = new Mapper();
        var student = new StudentClass { Name = "Ali", Age = 10 };
        var person = mapper.Map<PersonClass>(student);
        Assert.NotNull(student);
        Assert.Equal(person.Name, student.Name);
        Assert.Equal(person.Age, student.Age);
    }
}

file record Person(string Name, int Age);
file class PersonMapper
{

}
file record StudentRecord(string Name, int Age, string? Major) : IEntity;