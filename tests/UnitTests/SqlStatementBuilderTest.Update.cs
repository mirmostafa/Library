using Library.Data.SqlServer;
using Library.Exceptions.Validations;

namespace UnitTests;

[Trait("Category", "SQL Data Access")]
public sealed class UpdateStatementBuilderTest
{
    [Fact]
    public void CreateAllUpdate()
    {
        var actual = SqlStatementBuilder
                        .Update("dbo.Person")
                        .Set("Name", "Ali");
        var statement = actual.Build();
    }

    [Fact]
    public void CreateFullUpdate()
    {
        var actual = SqlStatementBuilder
                        .Update("dbo.Person")
                        .Set("Name", "Ali")
                        .Set("Age", 5)
                        .Where("Id = 5");
        var statement = actual.Build();
    }

    [Fact]
    public void CreateSetListUpdate()
    {
        var actual = SqlStatementBuilder
                        .Update("dbo.Person")
                        .Set(("Name", "Ali"), ("Age", 5));
        var statement = actual.Build();
    }

    [Fact]
    public void CreateTableUpdate()
    {
        var actual = SqlStatementBuilder
                        .Update()
                        .Table("dbo.Person")
                        .Set(("Name", "Ali"), ("Age", 5));
        var statement = actual.Build();
    }
}