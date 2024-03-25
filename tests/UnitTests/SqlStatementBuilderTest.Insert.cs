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
    public void SimpleReturnId()
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

    [Fact]
    public void ForceFormatValues()
    {
        var expected = @$"INSERT INTO [Employee]
    ([Name], [Age])
    VALUES (N'Mohammad', 45)";

        var actual = SqlStatementBuilder.Insert()
            .Into("Employee")
            .Values(("Name", "N'Mohammad'"), ("Age", 45))
            .ForceFormatValues(false)
            .Build();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ForceFormatValuesInvalidFormat()
    {
        var expected = @$"INSERT INTO [Employee]
    ([Name], [Age])
    VALUES (Mohammad, 45)";

        var actual = SqlStatementBuilder.Insert()
            .Into("Employee")
            .Values(("Name", "Mohammad"), ("Age", 45))
            .ForceFormatValues(false)
            .Build();

        Assert.Equal(expected, actual);
    }
}