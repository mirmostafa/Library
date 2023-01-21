using Library.Data.Markers;

namespace UnitTest.Models;

internal class PersonClass
{
    public int Age { get; set; }
    public string? Name { get; set; }
}

internal record PersonRecord(string? Name, int Age);

internal class StudentClass : PersonClass, IEntity
{
    public string? Major { get; set; }
}

internal record StudentRecord(string Name, int Age, string? Major) : IEntity;