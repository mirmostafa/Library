using Library.Data.SqlServer;

namespace UnitTests;

[Trait("Category", nameof(Library.Data.SqlServer))]
[Trait("Category", nameof(SqlStatementBuilder))]
public sealed class InsertStatementBuilderTest
{
    [Fact]
    public void SimpleInsertCommand()
    {
        var expected = @$"INSERT INTO [Employee]
    ([Name], [Age])
    VALUES (N'Mohammad', 45)";
        var actual = SqlStatementBuilder.Insert()
            .Into("Employee")
            .Values(("Name", "Mohammad"), ("Age", 45))
            .Build();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void SimpleInsertCommandAddId()
    {
        var expected = @$"INSERT INTO [Employee]
    ([Name], [Age])
    VALUES (N'Mohammad', 45);
SELECT SCOPE_IDENTITY();";
        var actual = SqlStatementBuilder.Insert()
            .Into("Employee")
            .Values(("Name", "Mohammad"), ("Age", 45))
            .ReturnId()
            .Build();
        Assert.Equal(expected, actual);
    }
}