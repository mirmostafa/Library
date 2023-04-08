using Library.Data.SqlServer;

namespace UnitTests;

public sealed class DeleteStatementBuilderTest
{
    [Fact]
    public void CreateDeleteFullTest()
    {
        var actual = SqlStatementBuilder
                        .Delete()
                        .Where("Id = 5")
                        .From("Person")
                        .Build();
        var expected = @"DELETE FROM [Person]
    WHERE Id = 5";
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CreateDeleteWhereTest()
    {
        var actual = SqlStatementBuilder
                        .Delete("Person")
                        .Where("Id = 5")
                        .Build();
        var expected = @"DELETE FROM [Person]
    WHERE Id = 5";
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CreateMinimalDeleteTest()
    {
        var actual = SqlStatementBuilder
                        .Delete("Person")
                        .Build();
        var expected = "DELETE FROM [Person]";
        Assert.Equal(expected, actual);
    }
}