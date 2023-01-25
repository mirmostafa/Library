using Library.Data.SqlServer;

namespace Library.UnitTest;

[TestClass]
public class DeleteStatementBuilderTest
{
    [TestMethod]
    public void CreateMinimalDeleteTest()
    {
        var actual = SqlStatementBuilder
                        .Delete("Person")
                        .Build();
        var expected = "DELETE FROM [Person]";
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void CreateDeleteWhereTest()
    {
        var actual = SqlStatementBuilder
                        .Delete("Person")
                        .Where("Id = 5")
                        .Build();
        var expected = @"DELETE FROM [Person]
    WHERE Id = 5";
        Assert.AreEqual(expected, actual);
    }
    [TestMethod]
    public void CreateDeleteFullTest()
    {
        var actual = SqlStatementBuilder
                        .Delete()
                        .Where("Id = 5")
                        .From("Person")
                        .Build();
        var expected = @"DELETE FROM [Person]
    WHERE Id = 5";
        Assert.AreEqual(expected, actual);
    }
}
