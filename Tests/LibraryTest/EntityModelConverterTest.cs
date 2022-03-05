using Library.Data.SqlServer;
using Library.Data.SqlServer.Builders;

namespace LibraryTest;

[TestClass]
public class EntityModelConverterTest
{
    [TestMethod]
    public void SqlTypeToNetType()
    {
        var columns = EntityModelConverterHelper.GetColumns<PersonEntity>().OrderBy(c => c.Order).ToList();
        Assert.AreEqual(5, columns.Count);
        Assert.AreEqual("LName", columns[2].Name);
    }

    [TestMethod]
    public void SqlStamentBuildByEntityModelTest()
    {
        var columns = EntityModelConverterHelper.GetColumns<PersonEntity>();
        var actual = SqlStatementBuilder
                        .Select()
                        .Columns(columns.OrderBy(c => c.Order).Select(c => c.Name))
                        .From(nameof(PersonEntity))
                        .Build();
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