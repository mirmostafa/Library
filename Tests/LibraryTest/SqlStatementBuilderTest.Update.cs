using Library.Data.SqlServer.Builders;

namespace LibraryTest;

[TestClass]
public class UpdateStatementBuilderTest
{
    [TestMethod]
    public void CreateFullUpdate()
    {
        var actual = SqlStatementBuilder
                        .Update("dbo.Person")
                        .Set("Name", "Ali")
                        .Set("Age", 5)
                        .Where("Id = 5")
                        .Build();

        var expected = @"Update [dbo].[Person]
    SET ([Name], [Age])
    VALUES (N'Ali', 5)
    WHERE Id = 5";

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void CreateAllUpdate()
    {
        var actual = SqlStatementBuilder
                        .Update("dbo.Person")
                        .Set("Name", "Ali")
                        .Build();

        var expected = @"Update [dbo].[Person]
    SET ([Name])
    VALUES (N'Ali')";

        Assert.AreEqual(expected, actual);
    }
    [TestMethod]
    public void CreateSetListUpdate()
    {
        var actual = SqlStatementBuilder
                        .Update("dbo.Person")
                        .Set(("Name", "Ali"), ("Age", 5))
                        .Build();

        var expected = @"Update [dbo].[Person]
    SET ([Name], [Age])
    VALUES (N'Ali', 5)";

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void CreateTableUpdate()
    {
        var actual = SqlStatementBuilder
                        .Update()
                        .Table("dbo.Person")
                        .Set(("Name", "Ali"), ("Age", 5))
                        .Build();

        var expected = @"Update [dbo].[Person]
    SET ([Name], [Age])
    VALUES (N'Ali', 5)";

        Assert.AreEqual(expected, actual);
    }
}