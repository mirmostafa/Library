#pragma warning disable CA1707 // Identifiers should not contain underscores
using Library.CodeGeneration;
using Library.Helpers.ConsoleHelper;

using Xunit.Abstractions;

namespace UnitTests;
public sealed class TypePathTest(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;
    private readonly string _sampleFullPath = "System.Linq.IQueryable<Library.Tests.UnitTests.TypePathTest>";
    private static readonly string[] _generics = ["System.Int32", "String"];

    [Fact]
    public void _05_SimpleTypeTest()
    {
        // Assign
        var expectedFullPath = "TypePathTest";
        var expectedName = "TypePathTest";
        string? expectedNameSpace = null;
        TypePath path = expectedFullPath;

        // Act
        var actualName = path.Name;
        var actualNameSpace = path.NameSpace;
        var actualFullPath = path.FullPath;

        // Assert
        this.Display(path);
        Assert.Equal(expectedName, actualName);
        Assert.Equal(expectedNameSpace, actualNameSpace);
        Assert.Equal(expectedFullPath, actualFullPath);
    }

    [Fact]
    public void _10_NormalTypeTest()
    {
        // Assign
        var expectedFullPath = "Library.Tests.UnitTests.TypePathTest";
        var expectedName = "TypePathTest";
        var expectedNameSpace = "Library.Tests.UnitTests";
        TypePath path = expectedFullPath;

        // Act
        var actualName = path.Name;
        var actualNameSpace = path.NameSpace;
        var actualFullPath = path.FullPath;

        // Assert
        this.Display(path);
        Assert.Equal(expectedName, actualName);
        Assert.Equal(expectedNameSpace, actualNameSpace);
        Assert.Equal(expectedFullPath, actualFullPath);
    }

    [Fact]
    public void _25_GenericTypeTest()
    {
        // Assign
        var expectedName = "IQueryable<Library.Tests.UnitTests.TypePathTest>";
        var expectedNameSpace = "System.Linq";
        var expectedGeneric = "Library.Tests.UnitTests.TypePathTest";
        var expectedClassName = "IQueryable";
        var expectedGenericName = "TypePathTest";
        var expectedGenericNameSpace = "Library.Tests.UnitTests";

        // Act
        TypePath path = this._sampleFullPath;
        var actualGeneric = path.Generics.FirstOrDefault();
        var actualName = path.Name;
        var actualNameSpace = path.NameSpace;
        var actualFullPath = path.FullPath;
        var allNameSpaces = path.GetNameSpaces().ToList();

        // Assert
        this.Display(path);
        this.Display(actualGeneric);
        Assert.Equal(expectedName, actualName);
        Assert.Equal(expectedNameSpace, actualNameSpace);
        Assert.Equal(this._sampleFullPath, actualFullPath);

        Assert.NotNull(actualGeneric);
        Assert.NotNull(actualGeneric.Name);
        Assert.NotNull(actualGeneric.NameSpace);
        Assert.Equal(expectedGeneric, actualGeneric);
        Assert.Equal(expectedGenericName, actualGeneric.Name);
        Assert.Equal(expectedGenericNameSpace, actualGeneric.NameSpace);
        Assert.Equal(2, allNameSpaces.Count);

    }

    [Fact]
    public void _20_SimpleGenericTypeTest()
    {
        var path = new TypePath(this._sampleFullPath);
        this.Display(path);
    }

    [Fact]
    public void _25_SimpleGenericWithAdditionalGenericsTypeTest()
    {
        var path = new TypePath(this._sampleFullPath, _generics);
        this.Display(path);
    }

    private void Display(TypePath? path)
    {
        if (path is null)
        {
            this._output.WriteLine($"Path is empty.");
            return;
        }
        this._output.WriteLine($"Path: {path}");
        this._output.WriteLine($"Name: {path.Name}");
        this._output.WriteLine($"NameSpace: {path.NameSpace}");
        this._output.WriteLine($"FullName: {path.FullName}");
        this._output.WriteLine($"FullPath: {path.FullPath}");
        if(path.GetNameSpaces().Any())
        {
            this._output.WriteLine("namespaces:");
            foreach (var ns in path.GetNameSpaces())
            {
                _output.WriteLine($"\t{ns}");
            }
        }
    }
}
#pragma warning restore CA1707 // Identifiers should not contain underscores
