using Library.Data.Markers;
using Library.Types;

namespace UnitTests.Models;

internal class PersonClass
{
    public Id Id { get; set; }
    public string? Name { get; set; }
    public int Age { get; set; }
    public string? LName { get; set; }
    public Id? AddressId { get; set; }
    public string? Address { get; set; }
}

internal class StudentClass : PersonClass, IEntity
{
    public string? Major { get; set; }
}