namespace UnitTests;

[Trait("Category", "Helpers")]
public class StringHelperTest
{
    private const string LONG_TEXT = "There 'a text', inside 'another text'. I want 'to find\" it.";
    private const string SSHORT_TEXT = "There 'a text', inside 'another text'.";
    private const string VERY_SHORT_TEXT = "text";

    [Theory]
    [InlineData("numbers", "number")]
    [InlineData("cases", "case")]
    [InlineData("handies", "handy")]
    [InlineData("people", "person")]
    [InlineData("children", "child")]
    public void _08_PluralizeTest(string pluralized, string single) => Assert.Equal(pluralized, StringHelper.Pluralize(single));

    [Theory]
    [InlineData("numbers", "number")]
    [InlineData("cases", "case")]
    [InlineData("handies", "handy")]
    [InlineData("people", "person")]
    [InlineData("children", "child")]
    public void _08_SingularizeTest(string pluralized, string single) => Assert.Equal(single, StringHelper.Singularize(pluralized));

    [Fact]
    public void AddStartEndTest()
    {
        const string MOHAMMAD = "Mohammad ";
        const string REZA = "reza";
        const string EXPECTED = "Mohammad reza";

        var actual1 = REZA.AddStart(MOHAMMAD);
        Assert.Equal(EXPECTED, actual1);

        var actual2 = MOHAMMAD.AddEnd(REZA);
        Assert.Equal(EXPECTED, actual2);
    }

    [Fact]
    public void AddTest()
    {
        var addAfter = VERY_SHORT_TEXT.Add(2);
        Assert.Equal(VERY_SHORT_TEXT + "  ", addAfter);

        var addBefore = VERY_SHORT_TEXT.Add(2, before: true);
        Assert.Equal("  " + VERY_SHORT_TEXT, addBefore);
    }

    [Fact]
    public void AllIndexesOfTest()
    {
        var test = "Mohammad";
        var indexes = test.AllIndexesOf("m");
        Assert.Equal(2, indexes.Count());
        Assert.Equal(5, indexes.ElementAt(1));
    }

    [Fact]
    public void CompactTest()
    {
        var arr = new[] { "Hello", "", "I ", "am", "", "Mohammad", ".", "", "I'm", "", "a ", "", "C# ", "", "Developer." };
        var actual = arr.Compact().ToArray();
        var expected = new[] { "Hello", "I ", "am", "Mohammad", ".", "I'm", "a ", "C# ", "Developer." };
        Assert.Equal(expected, actual);
    }

    [Trait("Category", "Optimizations")]
    [Theory]
    [InlineData(@"https://my.parsvds.com", "://", "/", "my.parsvds.com")]
    [InlineData(@"https://my.parsvds.com/", "://", "/", "my.parsvds.com")]
    [InlineData(@"https://my.parsvds.com/clientarea.php", "://", "/", "my.parsvds.com")]
    [InlineData(@"https://my.parsvds.com/clientarea.php?id={50E204C7-C1BC-4139-A339-100090D3E493}", "://", "/", "my.parsvds.com")]
    [InlineData(@"This is mohammad", "mohammad", "hello", "")]
    [InlineData(@"This is mohammad", "", "hello", "This is mohammad")]
    [InlineData(@"This is mohammad", "", "", "")]
    public void GetPhrase_StringOptimization(string text, string start, string end, string expected)
    {
        var actual = StringHelper.GetPhrase(text, start, end);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetPhraseTest()
    {
        var result1 = LONG_TEXT.GetPhrase(0, '\'', '\'');
        Assert.Equal("a text", result1);

        result1 = LONG_TEXT.GetPhrase(0, '\'');
        Assert.Equal("a text", result1);

        var result2 = LONG_TEXT.GetPhrase(1, '\'', '\'');
        Assert.Equal("another text", result2);

        var result3 = LONG_TEXT.GetPhrase(1, 'q');
        Assert.Null(result3);
    }

    [Fact]
    public void GetPhraseTest2()
    {
        var result4 = StringHelper.GetPhrase(null, 0, '\'', '\"');
        Assert.Null(result4);
    }

    [Fact]
    public void GetPhraseTest3()
    {
        var result4 = LONG_TEXT.GetPhrase(0, '\'', '\"');
        Assert.Equal("a text', inside 'another text'. I want 'to find", result4);
    }

    [Fact]
    public void HasPersianTest()
    {
        var hasPersian = "Hello. My اسم is Mohammad.";
        var has = StringHelper.HasPersian(hasPersian);
        Assert.True(has);

        var hasNotPersian = "Hello. My name is Mohammad";
        var hasNot = StringHelper.HasPersian(hasNotPersian);
        Assert.True(hasNot);
    }

    public static IEnumerable<object[]> IsNullOrEmptyData => new[]
    {
        new object[] { null!, true },
        new object[] { "", true },
        new object[] { string.Empty, true },
        new object[] { "Hello. My name is Mohammad", false },

    };

    [Theory]
    [MemberData(nameof(IsNullOrEmptyData))]
    public void IsNullOrEmptyTrue(string? text, bool expected)
    {
        var actual = text.IsNullOrEmpty();
        Assert.Equal(actual, expected);
    }

    [Theory]
    [InlineData("0", true)]
    [InlineData("100", true)]
    [InlineData("09124438164", true)]
    [InlineData("12345678.9", true)]
    [InlineData("-12345678.9", true)]
    [InlineData("12-345678.9", false)]
    [InlineData("12345678..9", false)]
    public void IsNumberValid(string number, bool expected)
    {
        var actual = StringHelper.IsNumber(number);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("0062614614", true)]
    [InlineData("0062614615", false)]
    public void NationalCodeTest(string nationalCode, bool expected)
    {
        var actual = StringHelper.IsValidIranianNationalCode(nationalCode);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void SeparateCamelCaseTest()
    {
        var factor = "Hello, My Name Is Mohammad. How Are You? Just Fine.";
        var merged = factor.Remove(" ");
        var actual = merged.SeparateCamelCase();
        Assert.Equal(actual, factor);
    }

    [Fact]
    public void SliceTest()
    {
        var sliceStr = "Mohammad Mirmostafa is a C# Developer.";

        var mohammad = sliceStr.Slice(0, 8);
        Assert.Equal("Mohammad", mohammad);

        var cSharpDeveloper = sliceStr.Slice(25);
        Assert.Equal("C# Developer.", cSharpDeveloper);
    }

    [Fact]
    public void SpaceExceptionTest()
        => Assert.Throws<ArgumentOutOfRangeException>(() => Assert.Equal("     ", StringHelper.Space(-5)));

    [Fact]
    public void SpaceTest()
        => Assert.Equal("     ", StringHelper.Space(5));

    [Theory]
    [InlineData("Hello, my name is Mohammad. How are you? Just fine. @mohammad.", new[] { "Hello, my name is ", "Mohammad. ", "How are you? ", "Just fine. @mohammad." })]
    public void SplitMergeTest(string text, string[] expected)
    {
        var actual = text.SplitCamelCase();
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(@"Server=myServerAddress;Database=myDatabase;Trusted_Connection=True;", "Database", "MyDatabase")]
    public void SplitPairTest(string text, string slice, string expected)
    {
        var pairs = StringHelper.SplitPair(text);
        (_, var result) = pairs.Where(kv => kv.Key == slice).Single();
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TruncateTest()
    {
        var result = LONG_TEXT.Truncate(LONG_TEXT.Length - 5);
        Assert.Equal("There", result);
    }
}