namespace UnitTests;

//[MemoryDiagnoser]

public class StringHelperTest
{
    private const string LONG_TEXT = "There 'a text', inside 'another text'. I want 'to find\" it.";
    private const string SSHORT_TEXT = "There 'a text', inside 'another text'.";
    private const string VERY_SHORT_TEXT = "text";

    //[Benchmark]
    public void _WasteTimeToWarmUpBenchmark()
        => Thread.Sleep(3000);

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

    [Fact]
    public void IsNullOrEmptyTest()
    {
        var emptyStr = string.Empty;
        var sampleStr = "Sample String";

        var emptyStrIsEmpty = emptyStr.IsNullOrEmpty();
        var sampleStrIsEmpty = sampleStr.IsNullOrEmpty();

        Assert.True(emptyStrIsEmpty, "Hey, Why did you change it?😡 I was working like a charm!");
        Assert.False(sampleStrIsEmpty);
    }

    [Fact]
    public void IsNumberTest()
    {
        var numStr = "123456789";
        var numStrIsNumber = StringHelper.IsNumber(numStr);

        var invNumStr = "12345678..9";
        var invNumStrIsNumber = StringHelper.IsNumber(invNumStr);

        Assert.True(numStrIsNumber);
        Assert.False(invNumStrIsNumber);
    }

    [Fact]
    public void NationalCodeTest()
    {
        var nc1 = "0062614614";
        var isValid1 = StringHelper.IsValidIranianNationalCode(nc1);

        Assert.True(isValid1);
    }

    [Fact]
    public void NationalCodeTest2()
    {
        var nc2 = "0062614615";
        var isValid2 = StringHelper.IsValidIranianNationalCode(nc2);

        Assert.False(isValid2);
    }

    [Fact]
    public void NationalCodeTest3()
    {
        var nc3 = "006261461";
        var isValid3 = StringHelper.IsValidIranianNationalCode(nc3);

        Assert.False(isValid3);
    }

    [Fact]
    public void PluralizeTest()
    {
        Assert.Equal("numbers", StringHelper.Pluralize("number"));
        Assert.Equal("cases", StringHelper.Pluralize("case"));
        Assert.Equal("handies", StringHelper.Pluralize("handy"));
        Assert.Equal("people", StringHelper.Pluralize("person"));
        Assert.Equal("children", StringHelper.Pluralize("child"));
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
    public void SingularizeTest()
    {
        Assert.Equal("number", StringHelper.Singularize("numbers"));
        Assert.Equal("case", StringHelper.Singularize("cases"));
        Assert.Equal("handy", StringHelper.Singularize("handies"));
        Assert.Equal("person", StringHelper.Singularize("people"));
        Assert.Equal("child", StringHelper.Singularize("children"));
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

    [Fact]
    public void SplitMergeTest()
    {
        var test = "Hello, my name is Mohammad. How are you? Just fine. @mohammad.";
        var actual = test.SplitCamelCase();
        var expected = new[] { "Hello, my name is ", "Mohammad. ", "How are you? ", "Just fine. @mohammad." };
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void SplitPairTest()
    {
        var cs = @"Server=myServerAddress;Database=myDatabase;Trusted_Connection=True;";
        var pairs = StringHelper.SplitPair(cs);
        (_, var value) = pairs.Where(kv => kv.Key == "Database").Single();
        Assert.Equal("myDatabase", value);
    }

    [Fact]
    public void TruncateTest()
    {
        var result = LONG_TEXT.Truncate(LONG_TEXT.Length - 5);
        Assert.Equal("There", result);
    }
}