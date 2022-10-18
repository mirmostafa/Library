using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnitTest;

internal record struct PersonRecrd(string Name, DateOnly DateOfBirth, Gender Gender)
{
    public int Age => DateTime.Now.Year - this.DateOfBirth.Year;

    public void Desconstruct(out string name, out int age)
        => (name, age) = (this.Name, this.Age);
}

internal enum Gender
{
    None,
    Male,
    Female,
    Unknown
}

[Table("Person", Schema = "dbo")]
internal class PersonEntity
{
    [ForeignKey(nameof(AddressId))]
    public AddressEntity? Address { get; set; }

    [Column(Order = 3)]
    public long AddressId { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string? FullName { get; set; }

    [Key]
    [Column(Order = 0)]
    public long Id { get; set; }

    [Column("LName", Order = 2)]
    public string? LastName { get; set; }

    [Column(Order = 1)]
    public string? Name { get; set; }
}

[Table("Address", Schema = "dbo")]
internal class AddressEntity
{
}