using Library.Data.Markers;

namespace UnitTest.Models;

internal class PersonClass
{
    public int Age { get; set; }
    public string? Name { get; set; }
}

internal record PersonRecord(string? Name, int Age);

internal class StudenClass : PersonClass, IEntity
{
    public string? Major { get; set; }
}

internal record StudenRecord(string Name, int Age, string? Major) : IEntity;