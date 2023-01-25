using Library.Data.SqlServer;

namespace Library.UnitTest;

[TestClass]
public class SelectStatementBuilderTest
{
    [TestMethod]
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
        Assert.AreEqual(specQueryExpected, specQueryActual);
    }

    [TestMethod]
    public void _02_BuildMinimialFromSelectTest()
    {
        var specQueryActual = SqlStatementBuilder
                        .Select("Person")
                        .Build();
        var specQueryExpected = @"SELECT *
    FROM [Person]";
        Assert.AreEqual(specQueryExpected, specQueryActual);
    }

    [TestMethod]
    public void _03_BuildStandardMinimizeSelectTest()
    {
        var specQueryActual = SqlStatementBuilder
                        .Select()
                        .Star()
                        .From("Person")
                        .Build();
        var specQueryExpected = @"SELECT *
    FROM [Person]";
        Assert.AreEqual(specQueryExpected, specQueryActual);
    }

    [TestMethod]
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
        Assert.AreEqual(specQueryExpected, actual);
    }

    [TestMethod]
    public void BuildAddListColSelectTest()
    {
        var specQueryActual = SqlStatementBuilder
                        .Select()
                        .AddColumns(new[] { "Id", "Name", "Age" })
                        .From("Person")
                        .Build();
        var specQueryExpected = @"SELECT [Id], [Name], [Age]
    FROM [Person]";
        Assert.AreEqual(specQueryExpected, specQueryActual);
    }

    [TestMethod]
    [ExpectedException(typeof(Exceptions.Validations.NullValueValidationException))]
    public void BuildAscendingFromSelectTest()
    {
        var specQueryActual = SqlStatementBuilder
                        .Select("Person")
                        .Ascending()
                        .Build();
        var specQueryExpected = @"SELECT *
    FROM [Person]";
        Assert.AreEqual(specQueryExpected, specQueryActual);
    }

    [TestMethod]
    public void BuildSelectAddColumnTest()
    {
        var specQueryActual = SqlStatementBuilder
                        .Select("Person")
                        .Columns("Id")
                        .AddColumn("Name", "Age")
                        .Build();
        var specQueryExpected = @"SELECT [Id], [Name], [Age]
    FROM [Person]";
        Assert.AreEqual(specQueryExpected, specQueryActual);
    }

    [TestMethod]
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
        Assert.AreEqual(specQueryExpected, actual);
    }

    [TestMethod]
    public void BuildSelectSpecTest()
    {
        var specQueryActual = SqlStatementBuilder
                        .Select("Person")
                        .Columns((new[] { "Id", "Name", "Age" }).AsEnumerable())
                        .Build();
        var specQueryExpected = @"SELECT [Id], [Name], [Age]
    FROM [Person]";
        Assert.AreEqual(specQueryExpected, specQueryActual);
    }

    [TestMethod]
    public void BuildStarSelectTest()
    {
        var specQueryActual = SqlStatementBuilder
                        .Select("Person")
                        .Star()
                        .Build();
        var specQueryExpected = @"SELECT *
    FROM [Person]";
        Assert.AreEqual(specQueryExpected, specQueryActual);
    }
}