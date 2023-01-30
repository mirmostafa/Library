using Library.Data.SqlServer;
using Library.Exceptions.Validations;

namespace UnitTests;


public class UpdateStatementBuilderTest
{
    [Fact]
    public void CreateFullUpdate()
    {
        var actual = SqlStatementBuilder
                        .Update("dbo.Person")
                        .Set("Name", "Ali")
                        .Set("Age", 5)
                        .Where("Id = 5");
        Assert.Throws<NoItemValidationException>(() => actual.Build());
    }

    [Fact]
    public void CreateAllUpdate()
    {
        var actual = SqlStatementBuilder
                        .Update("dbo.Person")
                        .Set("Name", "Ali");
        Assert.Throws<NoItemValidationException>(()=>actual.Build());
    }
    [Fact]
    public void CreateSetListUpdate()
    {
        var actual = SqlStatementBuilder
                        .Update("dbo.Person")
                        .Set(("Name", "Ali"), ("Age", 5));
        Assert.Throws<NoItemValidationException>(() => actual.Build());
    }

    [Fact]
    public void CreateTableUpdate()
    {
        var actual = SqlStatementBuilder
                        .Update()
                        .Table("dbo.Person")
                        .Set(("Name", "Ali"), ("Age", 5));
        Assert.Throws<NoItemValidationException>(() => actual.Build());
    }
}