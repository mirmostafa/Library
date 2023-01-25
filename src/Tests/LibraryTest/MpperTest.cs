using Library.Data.Markers;
using Library.Mapping;
using Library.UnitTest.Models;

namespace Library.UnitTest;

[TestClass]
public class MapperTest
{
    [TestMethod]
    public void RecordMappingTest()
    {
        var mapper = new Mapper();
        var student = new StudentRecord("Ali", 5, "Math");
        var person = mapper.Map<PersonClass>(student);
        Assert.IsNotNull(student);
        Assert.AreEqual(person.Name, student.Name);
        Assert.AreEqual(person.Age, student.Age);
    }

    [TestMethod]
    public void ReverseTest()
    {
        var mapper = new Mapper();
        var person = new PersonClass { Name = "Ali", Age = 10 };
        var student = mapper.Map<StudentClass>(person).ForMember(x => x.Major = "Math");
        Assert.IsNotNull(student);
        Assert.AreEqual(person.Name, student.Name);
        Assert.AreEqual(person.Age, student.Age);
        Assert.AreEqual("Math", student.Major);
    }

    [TestMethod]
    public void StraightForwardTest()
    {
        var mapper = new Mapper();
        var student = new StudentClass { Name = "Ali", Age = 10 };
        var person = mapper.Map<PersonClass>(student);
        Assert.IsNotNull(student);
        Assert.AreEqual(person.Name, student.Name);
        Assert.AreEqual(person.Age, student.Age);
    }
}

file record StudentRecord(string Name, int Age, string? Major) : IEntity;