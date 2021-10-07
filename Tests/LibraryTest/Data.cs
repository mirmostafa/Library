using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryTest;

internal record struct Person(string Name, int Age);

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
    [Key]
    [Column(Order = 0)]
    public long Id { get; set; }

    [Column(Order = 1)]
    public string? Name { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string? FullName { get; set; }

    [Column(Order = 3)]
    public long AddressId { get; set; }

    [ForeignKey(nameof(AddressId))]
    public AddressEntity? Address { get; set; }

    [Column("LName", Order = 2)]
    public string? LastName { get; set; }
}

[Table("Address", Schema = "dbo")]
internal class AddressEntity
{

}