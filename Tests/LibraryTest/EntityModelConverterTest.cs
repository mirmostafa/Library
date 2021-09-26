using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Library.Data.SqlServer;
using Library.Data.SqlServer.Builders.Builders;

namespace LibraryTest;

[TestClass]
public class EntityModelConverterTest
{
    [TestMethod]
    public void SqlTypeToNetType()
    {
        var columns = EntityModelConverter.GetColumns<PersonEntity>().OrderBy(c => c.Order).ToList();
        Assert.AreEqual(5, columns.Count);
        Assert.AreEqual("LName", columns[2].Name);
    }

    [TestMethod]
    public void SqlStamentBuildByEntityModelTest()
    {
        var columns = EntityModelConverter.GetColumns<PersonEntity>();
        var builder = SqlStatementBuilder
                             .Select()
                             .Columns(columns.OrderBy(c => c.Order).Select(c => c.Name))
                             .From(nameof(PersonEntity));
        var actual = builder.Build();
        var expected = @"SELECT [Id], [Name], [LName], [AddressId], [Address]
    FROM [PersonEntity]";
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void CreateSelectByEntityTest()
    {
        var actual = SqlStatementBuilder
                        .Select<PersonEntity>()
                        .Build();
        var expected = @"SELECT [Id], [Name], [LName], [AddressId], [Address]
    FROM [dbo].[Person]";
        Assert.AreEqual(expected, actual);
    }
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
    public Address? Address { get; set; }

    [Column("LName", Order = 2)]
    public string? LastName { get; set; }
}

internal class Address
{

}