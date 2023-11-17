using System;

using Library.Data.SqlServer;
using Library.Exceptions.Validations;

using Xunit;

namespace UnitTests;

[Trait("Category", nameof(Library.Data.SqlServer))]
[Trait("Category", nameof(SqlStatementBuilder))]
public sealed class SelectStatementBuilderTest
{
    [Fact]
    public void _01_BuildFullSelectTest()
    {
        var specQueryActual = SqlStatementBuilder
                        .Select()
                        .Columns("Id", "Name", "Age")
                        .From("dbo.Person")
                        .Where("Age > 0")
                        .OrderBy("Id")
                        .Descending()
                        .Build();
        var specQueryExpected = @"SELECT [Id], [Name], [Age]
    FROM [dbo].[Person]
    WHERE Age > 0
    ORDER BY [Id] DESC";
        Assert.Equal(specQueryExpected, specQueryActual);
    }

    [Fact]
    public void _02_BuildMinimalFromSelectTest()
    {
        var specQueryActual = SqlStatementBuilder
                        .Select("Person")
                        .Build();
        var specQueryExpected = @"SELECT *
    FROM [Person]";
        Assert.Equal(specQueryExpected, specQueryActual);
    }

    [Fact]
    public void _03_BuildStandardMinimizeSelectTest()
    {
        var specQueryActual = SqlStatementBuilder
                        .Select()
                        .Star()
                        .From("Person")
                        .Build();
        var specQueryExpected = @"SELECT *
    FROM [Person]";
        Assert.Equal(specQueryExpected, specQueryActual);
    }

    [Fact]
    public void _04_BuildSelectOrderByDescendingTest()
    {
        var specQueryActual = SqlStatementBuilder
                        .Select("Person")
                        .Star()
                        .OrderByDescending("Id");
        var actual = specQueryActual.Build();
        var specQueryExpected = @"SELECT *
    FROM [Person]
    ORDER BY [Id] DESC";
        Assert.Equal(specQueryExpected, actual);
    }

    [Fact]
    public void BuildAddListColSelectTest()
    {
        var specQueryActual = SqlStatementBuilder
            .Select()
            .AddColumns(["Id", "Name", "Age" ])
            .From("Person")
            .Build();
        var specQueryExpected = @"SELECT [Id], [Name], [Age]
    FROM [Person]";
        Assert.Equal(specQueryExpected, specQueryActual);
    }

    [Fact]
    public void BuildAscendingFromSelectTest()
    {
        var specQueryActual = SqlStatementBuilder
                        .Select("Person")
                        .Ascending();
        _ = Assert.Throws<ArgumentNullException>(() => specQueryActual.Build());
    }

    [Fact]
    public void BuildSelectAddColumnTest()
    {
        var specQueryActual = SqlStatementBuilder
                        .Select("Person")
                        .Columns("Id")
                        .AddColumn("Name", "Age")
                        .Build();
        var specQueryExpected = @"SELECT [Id], [Name], [Age]
    FROM [Person]";
        Assert.Equal(specQueryExpected, specQueryActual);
    }

    [Fact]
    public void BuildSelectClearOrderingTest()
    {
        var specQueryActual = SqlStatementBuilder
                        .Select("Person")
                        .AllColumns()
                        .OrderBy("Id")
                        .Descending();
        specQueryActual = specQueryActual
                        .ClearOrdering();
        var actual = specQueryActual.Build();
        var specQueryExpected = @"SELECT *
    FROM [Person]";
        Assert.Equal(specQueryExpected, actual);
    }

    [Fact]
    public void BuildSelectSpecTest()
    {
        var specQueryActual = SqlStatementBuilder
                        .Select("Person")
                        .Columns((new[] { "Id", "Name", "Age" }).AsEnumerable())
                        .Build();
        var specQueryExpected = @"SELECT [Id], [Name], [Age]
    FROM [Person]";
        Assert.Equal(specQueryExpected, specQueryActual);
    }

    [Fact]
    public void BuildStarSelectTest()
    {
        var specQueryActual = SqlStatementBuilder
                        .Select("Person")
                        .Star()
                        .Build();
        var specQueryExpected = @"SELECT *
    FROM [Person]";
        Assert.Equal(specQueryExpected, specQueryActual);
    }

    [Fact]
    public void CreateSelectGeneric()
    {
        var specQueryActual = SqlStatementBuilder.CreateSelect<Person>().Build();
        var specQueryExpected = @"SELECT [Id], [Name], [Age]
    FROM [Person]";
        Assert.Equal(specQueryExpected, specQueryActual);
    }
}

sealed class Person(long id, string name, int age)
{
    public long Id { get; } = id;
    public string Name { get; } = name;
    public int Age { get; } = age;
}