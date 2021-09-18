using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Library.Data.SqlServer;

namespace LibraryTest;

[TestClass]
public class EntityModelConverterTest
{
    [TestMethod]
    public void SqlTypeToNetType()
    {
        var columns = EntityModelConverter.GetColumns<Person>().OrderBy(c => c.Order).ToList();
        Assert.AreEqual(5, columns.Count);
        Assert.AreEqual("LName", columns[2].Name);
    }
}

internal class Person
{
    [Key]
    [Column(Order = 0)]
    public long Id { get; set; }

    [Column(Order = 1)]
    public string? Name { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string? Name2 { get; set; }

    [Column(Order = 3)]
    public long AddressId { get; set; }

    [ForeignKey(nameof(AddressId))]
    public Address? Address { get; set; }

    [Column("LName", Order = 2)]
    public string? LastName { get; set; }
}

internal class Address
{

}