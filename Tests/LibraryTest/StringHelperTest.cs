﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace LibraryTest;

//[MemoryDiagnoser]
[TestClass]
public class StringHelperTest
{
    private const string text = "There 'a text', inside 'another text'. I want 'to find\" it.";

    //[Benchmark]
    public void _WasteTimeToWarmUpBenchmark() => Thread.Sleep(3000);

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
        => Assert.AreEqual("     ", StringHelper.Space(-5));


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
    public void TruncateTest()
    {
        var result = text.Truncate(text.Length - 5);
        Assert.AreEqual("There", result);
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
    public void AddTest()
    {
        var actual = "Mohammad ".AddEnd("reza");
        var expected = "Mohammad reza";
        Assert.AreEqual(expected, actual);
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
    public void SplitMergeTest()
    {
        var test = "Hello, my name is Mohammad. How are you? Just fine. @mohammad.";
        var actual = test.SplitCamelCase();
        var expected = new[] { "Hello, my name is ", "Mohammad. ", "How are you? ", "Just fine. @mohammad." };
        Assert.That.AreEqual(expected, actual);
    }

    [TestMethod]
    public void SeparateCamelCaseTest()
    {
        var factor = "Hello, My Name Is Mohammad. How Are You? Just Fine.";
        var merged = factor.Remove(" ");
        var actual = merged.SeparateCamelCase();
        Assert.AreEqual(actual, factor);
    }
}
