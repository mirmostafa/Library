namespace LibraryTest;

[TestClass]
public class StringHelperTest
{
    private const string text = "There 'a text', inside 'another text'. I want 'to find\" it.";

    [TestMethod]
    public void IsNullOrEmptyTest()
    {
        var emptyStr = string.Empty;
        var sampleStr = "Sample String";

        var emptyStrIsEmpty = emptyStr.IsNullOrEmpty();
        var sampleStrIsEmpty = sampleStr.IsNullOrEmpty();

        Assert.IsTrue(emptyStrIsEmpty);
        Assert.IsFalse(sampleStrIsEmpty);
    }

    [TestMethod]
    public void IsNumberTest()
    {
        var numStr = "123456789";
        var numStrIsNumbber = numStr.IsNumber();

        var invNumStr = "12345678..9";
        var invNumStrIsNumber = invNumStr.IsNumber();

        Assert.IsTrue(numStrIsNumbber);
        Assert.IsFalse(invNumStrIsNumber);
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
    public void SinglarizeTest()
    {
        Assert.AreEqual("number", StringHelper.Singularize("numbers"));
        Assert.AreEqual("case", StringHelper.Singularize("cases"));
        Assert.AreEqual("handy", StringHelper.Singularize("handies"));
        Assert.AreEqual("person", StringHelper.Singularize("people"));
        Assert.AreEqual("child", StringHelper.Singularize("children"));
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
    public void SpaceTest()
        => Assert.AreEqual("     ", StringHelper.Space(5));

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void SpaceExceptionTest()
    {
        var expected = "     ";
        var actual = StringHelper.Space(-5);
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetPhraseTest()
    {
        var result1 = text.GetPhrase(0, '\'', '\'');
        Assert.AreEqual("a text", result1);

        result1 = text.GetPhrase(0, '\'');
        Assert.AreEqual("a text", result1);

        var result2 = text.GetPhrase(1, '\'', '\'');
        Assert.AreEqual("another text", result2);

        var result3 = text.GetPhrase(1, 'q');
        Assert.AreEqual(null, result3);
    }

    [TestMethod]
    public void GetPhraseTest2()
    {
        var result4 = text.GetPhrase(0, '\'', '\"');
        Assert.AreEqual("a text', inside 'another text'. I want 'to find\"", result4);
    }

    [TestMethod]
    public void NationalCodeTest()
    {
        var nc1 = "0062614614";
        var nc2 = "0062614615";
        var nc3 = "006261461";
        var isValid1 = StringHelper.IsValidIranianNationalCode(nc1);
        var isValid2 = StringHelper.IsValidIranianNationalCode(nc2);
        var isValid3 = StringHelper.IsValidIranianNationalCode(nc3);

        Assert.IsTrue(isValid1);
        Assert.IsFalse(isValid2);
        Assert.IsFalse(isValid3);
    }

    [TestMethod]
    public void TruncateTest()
    {
        var result = text.Truncate(text.Length - 5);
        Assert.AreEqual("There", result);
    }
}
