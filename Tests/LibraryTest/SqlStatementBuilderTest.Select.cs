using Library.Data.SqlServer;

namespace LibraryTest;

[TestClass]
public class SelectStatementBuilderTest
{
    [TestMethod]
    public void CreateFullSelectTest()
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
    public void CreateStarSelectTest()
    {
        var specQueryActual = SqlStatementBuilder
                        .Select("Person")
                        .Star()
                        .Build();
        var specQueryExpected = @"SELECT *
    FROM [Person]";
        Assert.AreEqual(specQueryExpected, specQueryActual);
    }

    [TestMethod]
    public void CreateStandardMinimizeSelectTest()
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
    public void CreateSelectClearOrderingTest()
    {
        var specQueryActual = SqlStatementBuilder
                        .Select("Person")
                        .AllColumns()
                        .OrderBy("Id")
                        .Descending()
                        .ClearOrdering()
                        .Build();
        var specQueryExpected = @"SELECT *
    FROM [Person]";
        Assert.AreEqual(specQueryExpected, specQueryActual);
    }

    [TestMethod]
    public void CreateSelectSpecTest()
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
    public void CreateSelectAddColumnTest()
    {
        var specQueryActual = SqlStatementBuilder
                        .Select("Person")
                        .AddColumn("Id", "Name", "Age")
                        .Build();
        var specQueryExpected = @"SELECT [Id], [Name], [Age]
    FROM [Person]";
        Assert.AreEqual(specQueryExpected, specQueryActual);
    }

    [TestMethod]
    public void CreateAddListColSelectTest()
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
    [ExpectedException(typeof(Library.Exceptions.Validations.NullValueValidationException))]
    public void CreateAscendingFromSelectTest()
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
    public void CreateMinimialFromSelectTest()
    {
        var specQueryActual = SqlStatementBuilder
                        .Select("Person")
                        .Build();
        var specQueryExpected = @"SELECT *
    FROM [Person]";
        Assert.AreEqual(specQueryExpected, specQueryActual);
    }
}
