using Library.CodeGeneration;
using Library.Exceptions.Validations;

using Xunit.Abstractions;

using static Library.CodeGeneration.TypePath;

namespace UnitTests;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2263:Prefer generic overload when type is known", Justification = "<Pending>")]
[Collection(nameof(TypePathTest))]
[Trait("Category", nameof(Library.CodeGeneration))]
[Trait("Category", nameof(TypePath))]
public sealed class TypePathTest(ITestOutputHelper output)
{
    private static readonly string[] _generics = ["System.Int32", "String"];
    private readonly ITestOutputHelper _output = output;
    private readonly string _sampleFullPath = "System.Linq.IQueryable<Library.Tests.UnitTests.TypePathTest>";

    public static IEnumerable<object[]> ParseData
    {
        get
        {
            // Straight forward
            yield return new object[]
            {
                "Person", new TypeData("Person", null, [], false)
            };
            // Simple namespace
            yield return new object[]
            {
                "System.Int32", new TypeData(nameof(Int32)!, typeof(int).Namespace!, [], false)
            };
            // Complex namespace
            yield return new object[]
            {
                "Sample.Data.Entities.Person", new TypeData("Person","Sample.Data.Entities", [], false)
            };
            // Straight forward nullable
            yield return new object[]
            {
                "Person?", new TypeData("Person",null, [], true)
            };
            // Simple namespace nullable
            yield return new object[]
            {
                "System.Int32?", new TypeData(nameof(Int32)!, typeof(int).Namespace!, [], true)
            };
            // Complex namespace nullable
            yield return new object[]
            {
                "Sample.Data.Entities.Person?", new TypeData("Person","Sample.Data.Entities", [], true)
            };

            // Simple generic
            yield return new object[]
            {
                "ValueType<Person>", new TypeData("ValueType", null, [new("Person", null, [], false)], false)
            };
            // Simple generic nullable 1
            yield return new object[]
            {
                "ValueType<Person>?", new TypeData("ValueType", null, [new("Person", null, [], false)], true)
            };
            // Simple generic nullable 2
            yield return new object[]
            {
                "ValueType<Person?>", new TypeData("ValueType", null, [new("Person", null, [], true)], false)
            };

            // Complex generic - real world
            yield return new object[]
            {
                "System.Threading.Tasks.Task<System.Int32>", new TypeData(nameof(Task), typeof(Task<int>).Namespace, [new(nameof(Int32), typeof(int).Namespace, [], false)], false)
            };

            // Complex generic nullable - real world
            yield return new object[]
            {
                "System.Threading.Tasks.Task<System.Int32?>", new TypeData(nameof(Task), typeof(Task<int>).Namespace, [new(nameof(Int32), typeof(int).Namespace, [], true)], false)
            };

            // More complex generic - real world
            yield return new object[]
            {
                "System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<System.Int32>>", new TypeData(nameof(Task), typeof(Task<IEnumerable<int>>).Namespace, [new(nameof(IEnumerable<int>), typeof(IEnumerable<int>).Namespace, [new(nameof(Int32), typeof(int).Namespace, [], false)], false)], false)
            };

            // More complex null generic - real world
            yield return new object[]
            {
                "System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<System.Int32?>>", new TypeData(nameof(Task), typeof(Task).Namespace, [new(nameof(IEnumerable<int>), typeof(IEnumerable<>).Namespace, [new(nameof(Int32), typeof(int).Namespace, [], true)], false)], false)
            };
        }
    }

    [Theory]
    [InlineData("int", "System.Int32")]
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
        var actual = TypePath.FromKeyword(keyword).FullPath;

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

        // Act
        TypePath path = expectedFullPath;
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

    [Fact]
    public void RealWorldReturnType_GenericTaskOfPerson()
    {
        var typePath = TypePath.New(typeof(Task<>).FullName!, ["Person"]);
        var actual = typePath.FullPath;
        var expected = "System.Threading.Tasks.Task<Person>";
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RealWorldReturnType_TaskOfPerson()
    {
        var typePath = TypePath.New(typeof(Task<>).FullName!, ["Person"]);
        var actual = typePath.FullPath;
        var expected = "System.Threading.Tasks.Task<Person>";
        Assert.Equal(expected, actual);
    }

    [Fact, Priority(20)]
    public void SimpleGenericTypeTest()
    {
        var path = new TypePath(this._sampleFullPath);
        this.Display(path);
    }

    [Fact, Priority(30)]
    public void SimpleGenericWithAdditionalGenericsTypeTest()
        => Assert.Throws<ValidationException>(() => new TypePath(this._sampleFullPath, _generics));

    [Fact, Priority(1)]
    public void SimpleTypeTest()
    {
        // Assign
        var expectedFullPath = "Person";
        var expectedName = "Person";
        var expectedNameSpace = string.Empty;

        // Act
        TypePath path = expectedFullPath;
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

    [Fact]
    public void TypeDataParse_Simple()
    {
        // Assign
        var fullPath = "System.Threading.Tasks.Task";
        var expected = fullPath;

        // Act
        var arg = TypePath.New(fullPath, []);
        var actual = arg.FullPath;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TypeDataParse_Simple_Clr_Generic_1()
    {
        // Assign
        var arg = TypePath.New<Task>([typeof(string)]);
        var expected = "System.Threading.Tasks.Task<System.String>";

        // Act
        var actual = arg.FullPath;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TypeDataParse_Simple_Clr_Generic_2()
    {
        // Assign
        var arg = TypePath.New(typeof(Task), [typeof(string)]);
        var expected = "System.Threading.Tasks.Task<System.String>";

        // Act
        var actual = arg.FullPath;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TypeDataParse_Simple_Clr_Generic_Invalid()
    {
        // Assign
        var type = typeof(Task<>).FullName!;
        var expected = "System.Threading.Tasks.Task";

        // Act
        var typePath = TypePath.New(type, []);
        var actual = typePath.FullPath;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TypeDataParse_Simple_Clr_Generic_Nullable_1()
    {
        // Assign
        var expected = "System.Threading.Tasks.Task<System.Int32?>";

        // Act
        var arg = TypePath.New<Task>([typeof(int?)]);
        var actual = arg.FullPath;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TypeDataParse_Simple_Clr_Generic_Nullable_2()
    {
        // Assign
        var expected = "System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<System.Int32?>>";

        // Act
        var arg = TypePath.New<Task>([typeof(IEnumerable<int?>)]);
        var actual = arg.FullPath;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TypeDataParse_Simple_Clr_Generic_Valid()
    {
        // Assign
        var mainType = typeof(Task<>).FullName!;
        var expected = "System.Threading.Tasks.Task<System.String>";

        // Act
        var arg = TypePath.New(mainType, [typeof(string).FullName!]);
        var actual = arg.FullPath;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TypeDataParse_Simple_Clr_Nullable()
    {
        // Assign
        var expected = "System.Int32?";
        var mainType = typeof(int?).FullName!;

        // Act
        var arg = TypePath.New(mainType, []);
        var actual = arg.FullPath;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TypeDataParse_Simple_Nullable()
    {
        // Assign
        var fullPath = "System.Int64?";
        var expected = fullPath;

        // Act
        var arg = TypePath.New(fullPath, []);
        var actual = arg.FullPath;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TypeDataParse_Simple_Type_1()
    {
        // Assign
        var arg = TypePath.New<Task>();
        var expected = "System.Threading.Tasks.Task";

        // Act
        var actual = arg.FullPath;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TypeDataParse_Simple_Type_2()
    {
        // Assign
        var arg = TypePath.New(typeof(Task));
        var expected = "System.Threading.Tasks.Task";

        // Act
        var actual = arg.FullPath;

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
    public void WithNullable(string path, bool isNullable)
    {
        var tp = TypePath.New(path);
        var tp1 = tp.WithNullable(isNullable);

        Assert.Equal(isNullable, tp1.IsNullable);
    }

    [Theory]
    [MemberData(nameof(ParseData))]
    internal void Parse(string fullPath, TypeData typeData)
    {
        var actual = fullPath;
        var expected = new TypePath(typeData).FullPath;

        Assert.Equal(expected, actual);
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