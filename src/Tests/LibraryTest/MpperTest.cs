using Library.Data.Markers;
using Library.Mapping;

namespace UnitTest;

[TestClass]
public class MapperTest
{
    [TestMethod]
    public void RecordMappingTest()
    {
        var mapper = new Mapper();
        var student = new StudenRecord("Ali", 5, "Math");
        var person = mapper.Map<PersonRecord>(student, () => new(null, 0));
        Assert.IsNotNull(student);
        Assert.AreEqual(person.Name, student.Name);
        Assert.AreEqual(person.Age, student.Age);
    }

    [TestMethod]
    public void ReverseTest()
    {
        var mapper = new Mapper();
        var person = new PersonClass { Name = "Ali", Age = 10 };
        var student = mapper.Map<StudenClass>(person).ForMember(x => x.Major = "Math");
        Assert.IsNotNull(student);
        Assert.AreEqual(person.Name, student.Name);
        Assert.AreEqual(person.Age, student.Age);
        Assert.AreEqual("Math", student.Major);
    }

    [TestMethod]
    public void StraightForwardTest()
    {
        var mapper = new Mapper();
        var student = new StudenClass { Name = "Ali", Age = 10 };
        var person = mapper.Map<PersonClass>(student);
        Assert.IsNotNull(student);
        Assert.AreEqual(person.Name, student.Name);
        Assert.AreEqual(person.Age, student.Age);
    }
}

internal class PersonClass
{
    public int Age { get; set; }
    public string? Name { get; set; }
}

internal record PersonRecord(string? Name, int Age);

internal class StudenClass : IEntity
{
    public int Age { get; set; }
    public string? Major { get; set; }
    public string? Name { get; set; }
}

internal record StudenRecord(string Name, int Age, string? Major) : IEntity;