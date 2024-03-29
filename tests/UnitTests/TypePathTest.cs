﻿using Library.CodeGeneration;

using Xunit.Abstractions;

namespace UnitTests;

[Collection(nameof(TypePathTest))]
[Trait("Category", nameof(Library.CodeGeneration))]
[Trait("Category", nameof(TypePath))]
public sealed class TypePathTest(ITestOutputHelper output)
{
    private static readonly string[] _generics = ["System.Int32", "String"];
    private readonly ITestOutputHelper _output = output;
    private readonly string _sampleFullPath = "System.Linq.IQueryable<Library.Tests.UnitTests.TypePathTest>";

    [Theory]
    [InlineData("int", "System.Int32")]
    [InlineData("int?", "System.Int32?")]
    [InlineData("string", "System.String")]
    [InlineData("string?", "System.String?")]
    [InlineData("Person", "Test.Person")]
    [InlineData("Person?", "Test.Person?")]
    public void AsKeyword(string keyword, string fullPath)
    {
        var expected = keyword;
        var actual = TypePath.New(fullPath).AsKeyword();

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("int", "System.Int32")]
    [InlineData("int?", "System.Int32?")]
    [InlineData("string", "System.String")]
    [InlineData("string?", "System.String?")]
    [InlineData("System.String?", "System.String?")]
    [InlineData("Test.Person?", "Test.Person?")]
    public void FromKeyword(string keyword, string fullPath)
    {
        var expected = fullPath;
        var actual = TypePath.New(keyword).FullPath;

        Assert.Equal(expected, actual);
    }

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

    [Theory]
    [InlineData("int?", true)]
    [InlineData("string?", true)]
    [InlineData("Test.Person?", true)]
    [InlineData("System.Collection.IEnumerable<int?>?", true)]
    [InlineData("System.Collection.IEnumerable<int?>", false)]
    [InlineData("System.Collection.IEnumerable<int>?", true)]
    public void NullabilityCheck(string type, bool expected)
    {
        // Assign
        var tp = TypePath.New(type);

        // Act
        var actual = tp.IsNullable;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("int?", true)]
    [InlineData("int?", false)]
    [InlineData("string?", true)]
    [InlineData("string?", false)]
    [InlineData("System.String?", false)]
    [InlineData("System.String?", true)]
    [InlineData("Test.Person?", true)]
    [InlineData("System.Collection.IEnumerable<int?>?", true)]
    [InlineData("System.Collection.IEnumerable<int?>?", false)]
    [InlineData("System.Collection.IEnumerable<int?>", true)]
    [InlineData("System.Collection.IEnumerable<int?>", false)]
    [InlineData("System.Collection.IEnumerable<int>?", true)]
    [InlineData("System.Collection.IEnumerable<int>?", false)]
    public void NullabilityCreate(string type, bool expected)
    {
        // Assign
        var tp = TypePath.New(type, isNullable: expected);

        // Act
        var actual = tp.IsNullable;

        // Assert
        Assert.Equal(expected, actual);
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
        var expectedNameSpace = string.Empty;
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

    [Theory]
    [InlineData("System.Collections.Generic.List<Test.HumanResources.PersonDto>?")]
    [InlineData("System.Collections.Generic.List<Test.HumanResources.PersonDto?>?")]
    public void SpecificTest1(string typeFullPath)
    {
        var expected = typeFullPath;

        var actual = TypePath.New(in typeFullPath);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("int?", true)]
    [InlineData("int?", false)]
    [InlineData("string?", true)]
    [InlineData("string?", false)]
    [InlineData("System.String?", false)]
    [InlineData("System.String?", true)]
    [InlineData("Test.Person?", true)]
    [InlineData("System.Collection.IEnumerable<int?>?", true)]
    [InlineData("System.Collection.IEnumerable<int?>?", false)]
    [InlineData("System.Collection.IEnumerable<int?>", true)]
    [InlineData("System.Collection.IEnumerable<int?>", false)]
    [InlineData("System.Collection.IEnumerable<int>?", true)]
    [InlineData("System.Collection.IEnumerable<int>?", false)]
    public void WithNullable(string path, bool isNullable)
    {
        var tp = TypePath.New(path);
        var tp1 = tp.WithNullable(isNullable);

        Assert.Equal(isNullable, tp1.IsNullable);
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