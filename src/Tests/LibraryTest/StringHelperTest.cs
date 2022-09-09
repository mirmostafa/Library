namespace UnitTest;

//[MemoryDiagnoser]
[TestClass]
public class StringHelperTest
{
    private const string LONG_TEXT = "There 'a text', inside 'another text'. I want 'to find\" it.";
    private const string SSHORT_TEXT = "There 'a text', inside 'another text'.";
    private const string VERY_SHORT_TEXT = "text";

    //[Benchmark]
    public void _WasteTimeToWarmUpBenchmark() 
        => Thread.Sleep(3000);

    [TestMethod]
    public void AddStartEndTest()
    {
        const string MOHAMMAD = "Mohammad ";
        const string REZA = "reza";
        const string EXPECTED = "Mohammad reza";

        var actual1 = REZA.AddStart(MOHAMMAD);
        Assert.AreEqual(EXPECTED, actual1);

        var actual2 = MOHAMMAD.AddEnd(REZA);
        Assert.AreEqual(EXPECTED, actual2);
    }

    [TestMethod]
    public void AddTest()
    {
        var addAfter = VERY_SHORT_TEXT.Add(2);
        Assert.AreEqual(VERY_SHORT_TEXT + "  ", addAfter);

        var addBefore = VERY_SHORT_TEXT.Add(2, before: true);
        Assert.AreEqual("  " + VERY_SHORT_TEXT, addBefore);
    }

    [TestMethod]
    public void AllIndexesOfTest()
    {
        var test = "Mohammad";
        var indexes = test.AllIndexesOf("m");
        Assert.AreEqual(2, indexes.Count());
        Assert.AreEqual(5, indexes.ElementAt(1));
    }

    [TestMethod]
    public void CompactTest()
    {
        var arr = new[] { "Hello", "", "I ", "am", "", "Mohammad", ".", "", "I'm", "", "a ", "", "C# ", "", "Developer." };
        var actual = arr.Compact().ToArray();
        var expected = new[] { "Hello", "I ", "am", "Mohammad", ".", "I'm", "a ", "C# ", "Developer." };
        Assert.That.AreEqual(expected, actual);
    }

    [TestMethod]
    public void HasPersianTest()
    {
        var hasPersian = "Hello. My اسم is Mohammad.";
        var has = StringHelper.HasPersian(hasPersian);
        Assert.IsTrue(has);

        var hasNotPersian = "Hello. My name is Mohammad";
        var hasNot = StringHelper.HasPersian(hasNotPersian);
        Assert.IsTrue(hasNot);
    }

    [TestMethod]
    public void GetPhraseTest()
    {
        var result1 = LONG_TEXT.GetPhrase(0, '\'', '\'');
        Assert.AreEqual("a text", result1);

        result1 = LONG_TEXT.GetPhrase(0, '\'');
        Assert.AreEqual("a text", result1);

        var result2 = LONG_TEXT.GetPhrase(1, '\'', '\'');
        Assert.AreEqual("another text", result2);

        var result3 = LONG_TEXT.GetPhrase(1, 'q');
        Assert.AreEqual(null, result3);
    }

    [TestMethod]
    public void GetPhraseTest2()
    {
        var result4 = LONG_TEXT.GetPhrase(0, '\'', '\"');
        Assert.AreEqual("a text', inside 'another text'. I want 'to find", result4);
    }

    [TestMethod]
    public void IsNullOrEmptyTest()
    {
        var emptyStr = string.Empty;
        var sampleStr = "Sample String";

        var emptyStrIsEmpty = emptyStr.IsNullOrEmpty();
        var sampleStrIsEmpty = sampleStr.IsNullOrEmpty();

        Assert.IsTrue(emptyStrIsEmpty, "Hey, Why did you change it?😡 I was working like a charm!");
        Assert.IsFalse(sampleStrIsEmpty);
    }

    [TestMethod]
    public void IsNumberTest()
    {
        var numStr = "123456789";
        var numStrIsNumbber = StringHelper.IsNumber(numStr);

        var invNumStr = "12345678..9";
        var invNumStrIsNumber = StringHelper.IsNumber(invNumStr);

        Assert.IsTrue(numStrIsNumbber);
        Assert.IsFalse(invNumStrIsNumber);
    }

    [TestMethod]
    public void NationalCodeTest()
    {
        var nc1 = "0062614614";
        var isValid1 = StringHelper.IsValidIranianNationalCode(nc1);

        Assert.IsTrue(isValid1);
    }

    [TestMethod]
    public void NationalCodeTest2()
    {
        var nc2 = "0062614615";
        var isValid2 = StringHelper.IsValidIranianNationalCode(nc2);

        Assert.IsFalse(isValid2);
    }

    [TestMethod]
    public void NationalCodeTest3()
    {
        var nc3 = "006261461";
        var isValid3 = StringHelper.IsValidIranianNationalCode(nc3);

        Assert.IsFalse(isValid3);
    }

    [TestMethod]
    public void PluralizeTest()
    {
        Assert.AreEqual("numbers", StringHelper.Pluralize("number"));
        Assert.AreEqual("cases", StringHelper.Pluralize("case"));
        Assert.AreEqual("handies", StringHelper.Pluralize("handy"));
        Assert.AreEqual("people", StringHelper.Pluralize("person"));
        Assert.AreEqual("children", StringHelper.Pluralize("child"));
    }

    [TestMethod]
    public void SeparateCamelCaseTest()
    {
        var factor = "Hello, My Name Is Mohammad. How Are You? Just Fine.";
        var merged = factor.Remove(" ");
        var actual = merged.SeparateCamelCase();
        Assert.AreEqual(actual, factor);
    }

    [TestMethod]
    public void SinglarizeTest()
    {
        Assert.AreEqual("number", StringHelper.Singularize("numbers"));
        Assert.AreEqual("case", StringHelper.Singularize("cases"));
        Assert.AreEqual("handy", StringHelper.Singularize("handies"));
        Assert.AreEqual("person", StringHelper.Singularize("people"));
        Assert.AreEqual("child", StringHelper.Singularize("children"));
    }

    [TestMethod]
    public void SliceTest()
    {
        var sliceStr = "Mohammad Mirmostafa is a C# Developer.";

        var mohammad = sliceStr.Slice(0, 8);
        Assert.AreEqual(mohammad, "Mohammad");

        var cShartDeveloper = sliceStr.Slice(25);
        Assert.AreEqual(cShartDeveloper, "C# Developer.");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void SpaceExceptionTest()
        => Assert.AreEqual("     ", StringHelper.Space(-5));

    [TestMethod]
    public void SpaceTest()
        => Assert.AreEqual("     ", StringHelper.Space(5));

    [TestMethod]
    public void SplitMergeTest()
    {
        var test = "Hello, my name is Mohammad. How are you? Just fine. @mohammad.";
        var actual = test.SplitCamelCase();
        var expected = new[] { "Hello, my name is ", "Mohammad. ", "How are you? ", "Just fine. @mohammad." };
        Assert.That.AreEqual(expected, actual);
    }

    [TestMethod]
    public void SplitPairTest()
    {
        var cs = @"Server=myServerAddress;Database=myDatabase;Trusted_Connection=True;";
        var pairs = StringHelper.SplitPair(cs);
        (_, var value) = pairs.Where(kv => kv.Key == "Database").Single();
        Assert.AreEqual("myDatabase", value);
    }

    [TestMethod]
    public void TruncateTest()
    {
        var result = LONG_TEXT.Truncate(LONG_TEXT.Length - 5);
        Assert.AreEqual("There", result);
    }
}