using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Library.Data.SqlServer;
using Library.Data.SqlServer.Builders;

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

    [TestMethod]
    public void SqlStamentBuiByEntityModelTest()
    {
        var columns = EntityModelConverter.GetColumns<Person>();
        var builder = Library.Data.SqlServer.Builders.SqlStatementBuilder
                             .Select()
                             .Columns(columns.Select(c => c.Name))
                             .From(nameof(Person));
        var actual = builder.Build();
        var expected = @"SELECT [Id], [Name], [AddressId], [Address], [LName]
    FROM [Person]";
        Assert.AreEqual(expected, actual);
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