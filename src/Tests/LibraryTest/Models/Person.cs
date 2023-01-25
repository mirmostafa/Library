using Library.Data.Markers;

namespace Library.UnitTest.Models;

internal class PersonClass
{
    public int Age { get; set; }
    public string? Name { get; set; }
}

internal class StudentClass : PersonClass, IEntity
{
    public string? Major { get; set; }
}