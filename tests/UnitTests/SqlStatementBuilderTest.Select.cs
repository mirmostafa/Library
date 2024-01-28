using Library.Data.SqlServer;

namespace UnitTests;

[Trait("Category", nameof(Library.Data.SqlServer))]
[Trait("Category", nameof(SqlStatementBuilder))]
public sealed class SelectStatementBuilderTest
{
    [Fact]
    public void BuildAddListColSelectTest()
    {
        var actual = SqlStatementBuilder
            .Select()
            .AddColumns([nameof(Person.Id), nameof(Person.FirstName), nameof(Person.LastName)])
            .From(nameof(Person))
            .Build();
        var expected = $@"SELECT [{nameof(Person.Id)}], [{nameof(Person.FirstName)}], [{nameof(Person.LastName)}]
    FROM [{nameof(Person)}]";
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void BuildAscendingFromSelectTest()
    {
        var specQueryActual = SqlStatementBuilder
                        .Select(nameof(Person))
                        .Ascending();
        _ = Assert.Throws<ArgumentNullException>(() => specQueryActual.Build());
    }

    [Fact]
    public void BuildFullSelectTest()
    {
        var specQueryActual = SqlStatementBuilder
                        .Select()
                        .Columns(nameof(Person.Id), nameof(Person.FirstName), nameof(Person.Age))
                        .From($"dbo.{nameof(Person)}")
                        .Where($"{nameof(Person.Age)} > 0")
                        .OrderBy(nameof(Person.Id))
                        .Descending()
                        .Build();
        var specQueryExpected = $@"SELECT [{nameof(Person.Id)}], [{nameof(Person.FirstName)}], [{nameof(Person.Age)}]
    FROM [dbo].[{nameof(Person)}]
    WHERE {nameof(Person.Age)} > 0
    ORDER BY [{nameof(Person.Id)}] DESC";
        Assert.Equal(specQueryExpected, specQueryActual);
    }

    [Fact]
    public void BuildMinimalFromSelectTest()
    {
        var specQueryActual = SqlStatementBuilder
                        .Select(nameof(Person))
                        .Build();
        var specQueryExpected = $@"SELECT *
    FROM [{nameof(Person)}]";
        Assert.Equal(specQueryExpected, specQueryActual);
    }

    [Fact]
    public void BuildSelectAddColumnTest()
    {
        var specQueryActual = SqlStatementBuilder
                        .Select(nameof(Person))
                        .Columns(nameof(Person.Id))
                        .AddColumn(nameof(Person.LastName), nameof(Person.Age))
                        .Build();
        var specQueryExpected = $@"SELECT [{nameof(Person.Id)}], [{nameof(Person.LastName)}], [{nameof(Person.Age)}]
    FROM [{nameof(Person)}]";
        Assert.Equal(specQueryExpected, specQueryActual);
    }

    [Fact]
    public void BuildSelectClearOrderingTest()
    {
        var specQueryActual = SqlStatementBuilder
                        .Select(nameof(Person))
                        .AllColumns()
                        .OrderBy(nameof(Person.Id))
                        .Descending();
        specQueryActual = specQueryActual
                        .ClearOrdering();
        var actual = specQueryActual.Build();
        var specQueryExpected = $@"SELECT *
    FROM [{nameof(Person)}]";
        Assert.Equal(specQueryExpected, actual);
    }

    [Fact]
    public void BuildSelectOrderByDescendingTest()
    {
        var specQueryActual = SqlStatementBuilder
                        .Select(nameof(Person))
                        .Star()
                        .OrderByDescending(nameof(Person.Id));
        var actual = specQueryActual.Build();
        var specQueryExpected = $@"SELECT *
    FROM [{nameof(Person)}]
    ORDER BY [{nameof(Person.Id)}] DESC";
        Assert.Equal(specQueryExpected, actual);
    }

    [Fact]
    public void BuildSelectSpecTest()
    {
        var specQueryActual = SqlStatementBuilder
                        .Select(nameof(Person))
                        .Columns([nameof(Person.Id), nameof(Person.FirstName), nameof(Person.Age)])
                        .Build();
        var specQueryExpected = $@"SELECT [{nameof(Person.Id)}], [{nameof(Person.FirstName)}], [{nameof(Person.Age)}]
    FROM [{nameof(Person)}]";
        Assert.Equal(specQueryExpected, specQueryActual);
    }

    [Fact]
    public void BuildStandardMinimizeSelectTest()
    {
        var specQueryActual = SqlStatementBuilder
                        .Select()
                        .Star()
                        .From(nameof(Person))
                        .Build();
        var specQueryExpected = $@"SELECT *
    FROM [{nameof(Person)}]";
        Assert.Equal(specQueryExpected, specQueryActual);
    }

    [Fact]
    public void BuildStarSelectTest()
    {
        var specQueryActual = SqlStatementBuilder
                        .Select(nameof(Person))
                        .Star()
                        .Build();
        var specQueryExpected = $@"SELECT *
    FROM [{nameof(Person)}]";
        Assert.Equal(specQueryExpected, specQueryActual);
    }
}

file sealed class Person(long id, string name, int age)
{
    public int Age { get; } = age;
    public string FirstName { get; } = name;
    public long Id { get; } = id;
    public string? LastName { get; } = name;
}