using Library.CodeGeneration;

using Xunit.Abstractions;

namespace UnitTests;

[Collection(nameof(TypePathTest))]
[Trait("Category", nameof(Library.CodeGeneration))]
public sealed class TypePathTest(ITestOutputHelper output)
{
    private static readonly string[] _generics = ["System.Int32", "String"];
    private readonly ITestOutputHelper _output = output;
    private readonly string _sampleFullPath = "System.Linq.IQueryable<Library.Tests.UnitTests.TypePathTest>";

    [Fact, Priority(25)]
    public void GenericTypeTest()
    {
        // Assign
        var expectedName = "IQueryable";
        var expectedNameSpace = "System.Linq";
        var expectedGeneric = "Library.Tests.UnitTests.TypePathTest";
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

    [Theory]
    [InlineData("System.String", "string", "")]
    [InlineData("System.Int32", "int", "")]
    [InlineData("System.Int64", "long", "")]
    [InlineData("System.Decimal", "decimal", "")]
    [InlineData("System.Single", "float", "")]
    [InlineData("System.Boolean", "bool", "")]
    [InlineData("Test.Person", "Person", "Test")]
    public void KeywordTest(string source, string name, string nameSpace)
    {
        var type = TypePath.New(source);
        var (n, ns) = type.ToKeyword();
        Assert.Equal(nameSpace, ns);
        Assert.Equal(name, n);
    }

    [Fact, Priority(2)]
    public void NormalTypeTest()
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

    [Fact, Priority(20)]
    public void SimpleGenericTypeTest()
    {
        var path = new TypePath(this._sampleFullPath);
        this.Display(path);
    }

    [Fact, Priority(35)]
    public void SimpleGenericWithAdditionalGenericsTypeTest()
    {
        var path = new TypePath(this._sampleFullPath, _generics);
        this.Display(path);
    }

    [Fact, Priority(1)]
    public void SimpleTypeTest()
    {
        // Assign
        var expectedFullPath = "TypePathTest";
        var expectedName = "TypePathTest";
        string expectedNameSpace = string.Empty;
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
        if (path.GetNameSpaces().Any())
        {
            this._output.WriteLine("namespaces:");
            foreach (var ns in path.GetNameSpaces())
            {
                this._output.WriteLine($"\t{ns}");
            }
        }
    }
}