using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Library.Data.SqlServer;

namespace UnitTest;

[TestClass]
public class SqlEntityTest
{
    [TestMethod]
    public void CreateSelectByEntityTest()
    {
        var actual = SqlStatementBuilder
                        .Select<Person>()
                        .Build();
        var expected = @"SELECT [Id], [Name], [LName], [AddressId], [Address]
    FROM [dbo].[Person]";
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void SqlStamentBuildByEntityModelTest()
    {
        var columns = SqlEntity.GetColumns<Person>();
        var actual = SqlStatementBuilder
                        .Select()
                        .Columns(columns.Select(c => c.Name))
                        .From(nameof(Person))
                        .Build();
        var expected = $@"SELECT [Id], [Name], [LName], [AddressId], [Address]
    FROM [{nameof(Person)}]";
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void SqlTypeToNetType()
    {
        var columns = SqlEntity.GetColumns<Person>().ToList();
        Assert.AreEqual(5, columns.Count);
        Assert.AreEqual("LName", columns[2].Name);
    }
}

[Table("Person", Schema = "dbo")]
internal class Person
{
    [ForeignKey(nameof(AddressId))]
    public Address? Address { get; set; }

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
internal class Address
{
}