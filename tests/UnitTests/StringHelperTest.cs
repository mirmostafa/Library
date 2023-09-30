using System.Collections;

namespace UnitTests;

[Trait("Category", "Helpers")]
public sealed class StringHelperTest
{
    private const string LONG_TEXT = "There 'a text', inside 'another text'. I want 'to find\" it.";
    private const char NULL_CHAR = default!;
    private const string SSHORT_TEXT = "There 'a text', inside 'another text'.";
    private const string VERY_SHORT_TEXT = "text";

    public static IEnumerable<object[]> IfNullData =>
        new List<object[]>
        {
            new object[] { null, "default value", "default value" },
            new object[] { "", "default value", "" },
            new object[] { "not null", "default value", "not null" },
            new object[] { " ", "default value", " " },
        };

    public static IEnumerable<object[]> IsNullOrEmptyData => new[]
        {
        new object[] { null!, true },
        new object[] { string.Empty, true },
        new object[] { "", true },
        new object[] { " ", false },
        new object[] { "Hello. My name is Mohammad", false },
    };

    [Fact]
    public void Add()
    {
        var addAfter = VERY_SHORT_TEXT.Add(2);
        Assert.Equal(VERY_SHORT_TEXT + "  ", addAfter);

        var addBefore = VERY_SHORT_TEXT.Add(2, before: true);
        Assert.Equal("  " + VERY_SHORT_TEXT, addBefore);
    }

    [Theory]
    [InlineData("Hello", 3, ' ', false, "Hello   ")]
    [InlineData("World", 2, '*', true, "**World")]
    [InlineData(null, 5, '-', false, null)]
    public void Add_ShouldAddCharactersToString(string? input, int count, char add, bool before, string? expected)
    {
        // Act
        var result = input.Add(count, add, before);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Add_ShouldPadLeft_WhenBeforeIsTrue()
    {
        // Arrange
        var s = "Hello";

        // Act
        var result = s.Add(3, before: true);

        // Assert
        Assert.Equal("   Hello", result);
    }

    [Fact]
    public void Add_ShouldPadRight_WhenBeforeIsFalse()
    {
        // Arrange
        var s = "Hello";

        // Act
        var result = s.Add(3);

        // Assert
        Assert.Equal("Hello   ", result);
    }

    [Fact]
    public void Add_ShouldReturnNull_WhenStringIsNull()
    {
        // Arrange
        string? s = null;

        // Act
        var result = s.Add(3);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Add_ShouldReturnSameString_WhenCountIsZero()
    {
        // Arrange
        var s = "Hello";

        // Act
        var result = s.Add(0);

        // Assert
        Assert.Equal(s, result);
    }

    [Fact]
    public void Add_ShouldUseSpecifiedChar_WhenAddIsProvided()
    {
        // Arrange
        var s = "Hello";

        // Act
        var result = s.Add(3, add: '*');

        // Assert
        Assert.Equal("Hello***", result);
    }

    [Theory]
    [InlineData("Start", "End", "StartEnd")]
    public void AddEnd(string text, string prefix, string expected)
    {
        var actual = text.AddEnd(prefix);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("End", "Start", "StartEnd")]
    public void AddStart(string text, string suffix, string expected)
    {
        var actual = text.AddStart(suffix);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void AllIndexesOf()
    {
        var test = "Mohammad";
        var indexes = test.AllIndexesOf("m");
        Assert.Equal(2, indexes.Count());
        Assert.Equal(5, indexes.ElementAt(1));
    }

    [Theory]
    [InlineData("Mohammad", "mhM", true)]
    [InlineData(null, "mhM", false)]
    [InlineData(null, null, false)]
    public void AnyCharInString(string str, string range, bool expected)
    {
        var actual = str.AnyCharInString(range);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void AnyCharInString_Exception()
    {
        var text = "Mohammad";
        string? range = null;
        _ = Assert.Throws<ArgumentNullException>(() => text.AnyCharInString(range));
    }

    [Theory]
    [InlineData("سلامﮎ", "سلامک")]
    [InlineData("سلامي", "سلامی")]
    public void ArabicCharsToPersian(string arabic, string expected)
    {
        var actual = StringHelper.ArabicCharsToPersian(arabic);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("This is a text", false)]
    [InlineData("this is a text", true)]
    public void CheckAllValidations(string text, bool expected)
    {
        var isLower = (char x) => x == char.ToLower(x);
        var actual = StringHelper.CheckAllValidations(text, isLower);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("This is a text", true)]
    [InlineData("this is a text", true)]
    [InlineData("THIS IS A TEXT", false)]
    public void CheckAnyValidations(string text, bool expected)
    {
        var isLower = (char x) => x == char.ToLower(x) && x != ' ';
        var actual = StringHelper.CheckAnyValidations(text, isLower);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [ClassData(typeof(CompactDataClass))]
    public void Compact_Array(string[] data, string[] expected)
    {
        var actual = StringHelper.Compact(data).ToArray();
        Assert.Equal(expected, actual);
    }

    [Theory]
    [ClassData(typeof(CompactDataClass))]
    public void Compact_Enumerable(IEnumerable<string> data, IEnumerable<string> expected)
    {
        var actual = data.Compact().ToArray();
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("hello", "hello", true, true)]
    [InlineData("hello", "hello", false, true)]
    [InlineData("hello", "heLLo", true, true)]
    [InlineData("hello", "heLLo", false, false)]
    public void CompareTo(string str1, string str2, bool ignoreCase, bool expected)
    {
        var actual = StringHelper.CompareTo(str1, str2, ignoreCase) == 0;
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(new string[] { "a", "b", "c" }, ",", "a,b,c")]
    [InlineData(new string[] { "hello", "world" }, " ", "hello world")]
    [InlineData(new string[] { "foo", "bar" }, null, "foobar")]
    public void ConcatAll_Should_Return_Correct_String(IEnumerable<string> strings, string sep, string expected)
    {
        // Arrange

        // Act
        var actual = StringHelper.ConcatAll(strings, sep);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(new string[] { "a", "b", "c" }, "abc")]
    [InlineData(new string[] { "hello", " ", "world" }, "hello world")]
    public void ConcatStringsTest(string[] values, string expected)
    {
        // Arrange

        // Act
        var actual = StringHelper.ConcatStrings(values);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Contains_Mine_ReturnsTrue_WhenMatchingCaseSensitiveStringExists()
    {
        // Arrange
        IEnumerable<string> array = new List<string> { "Apple", "Banana", "Cherry" };
        var str = "Banana";
        var ignoreCase = false;

        // Act
        var result = array.Contains(str, ignoreCase);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Contains_Mine_ReturnsTrue_WhenMatchingCaseInsensitiveStringExists()
    {
        // Arrange
        IEnumerable<string> array = new List<string> { "Apple", "Banana", "Cherry" };
        string str = "banana";
        bool ignoreCase = true;

        // Act
        bool result = array.Contains(str, ignoreCase);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Contains_Mine_ReturnsFalse_WhenCaseSensitiveStringDoesNotExist()
    {
        // Arrange
        IEnumerable<string> array = new List<string> { "Apple", "Banana", "Cherry" };
        string str = "Grape";
        bool ignoreCase = false;

        // Act
        bool result = array.Contains(str, ignoreCase);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Contains_Mine_ReturnsFalse_WhenCaseInsensitiveStringDoesNotExist()
    {
        // Arrange
        IEnumerable<string> array = new List<string> { "Apple", "Banana", "Cherry" };
        string str = "grape";
        bool ignoreCase = true;

        // Act
        bool result = array.Contains(str, ignoreCase);

        // Assert
        Assert.False(result);
    }


    [Theory]
    [InlineData("This is a test string.", new char[] { 'x', 'y', 'z' }, false)]
    [InlineData("This is a test string.", new char[] { 'a', 'b', 'c' }, true)]
    [InlineData("", new char[] { 'x', 'y', 'z' }, false)]
    [InlineData("This is a test string.", new char[] { 'T', 't', 's' }, true)]
    public void ContainsAnyCharTest(string str, char[] chars, bool expectedResult)
    {
        var result = StringHelper.ContainsAny(str, chars);
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData("Hello", new string[] { "He", "ll", "o" }, true)]
    [InlineData("Hello", new string[] { "he", "LL", "O" }, false)]
    [InlineData("Hello", new string[] { "Hi", "Bye", "No" }, false)]
    public void ContainsAnyStringArrayTest(string str, string[] array, bool expected)
    {
        // Arrange

        // Act
        var actual = StringHelper.ContainsAny(str, array);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("Hello", new string[] { "He", "ll", "o" }, true)]
    [InlineData("Hello", new string[] { "he", "LL", "O" }, false)]
    [InlineData("Hello", new string[] { "Hi", "Bye", "No" }, false)]
    public void ContainsAnyStringsTest(string str, string[] array, bool expected)
    {
        // Arrange

        // Act
        var actual = StringHelper.ContainsAny(str, array.AsEnumerable());

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(new string[] { "a", "b", "c" }, "b", true, true)]
    [InlineData(new string[] { "a", "b", "c" }, "B", false, false)]
    [InlineData(new string[] { "a", "b", "c" }, "d", true, false)]
    public void ContainsArrayTest(string[] array, string str, bool ignoreCase, bool expected)
    {
        // Arrange

        // Act
        var actual = StringHelper.Contains(array, str, ignoreCase);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("Hello", "He", true)]
    [InlineData("Hello", "he", true)]
    [InlineData("Hello", "Hi", false)]
    public void ContainsOfTest(string str, string target, bool expected)
    {
        // Arrange

        // Act
        var actual = StringHelper.ContainsOf(str, target);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(null, "test", true, false)]
    [InlineData("Hello", "hello", true, true)]
    [InlineData("Hello", "hello", false, false)]
    public void ContainsTest(string str, string value, bool ignoreCase, bool expected)
    {
        // Arrange

        // Act
        var actual = StringHelper.Contains(str, value, ignoreCase);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("", "")]
    [InlineData("سلام", "سلام")]
    [InlineData("يك", "یک")]
    [InlineData("كيف حالك؟", "کیف حالک؟")]
    public void CorrectUnicodeProblemTest(string text, string expected)
    {
        // Arrange Nothing to do

        // Act
        var actual = StringHelper.CorrectUnicodeProblem(text);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("This is a test string.", 'i', 3, 2)]
    [InlineData("This is a test string.", 'x', 0, 0)]
    [InlineData("", 'x', 0, 0)]
    [InlineData("This is a test string.", 's', 0, 4)]
    public void CountOfTest(string str, char c, int index, int expectedResult)
    {
        var result = str.CountOf(c, index);
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData("Hello World!", new object[] { "o", "d", "World!", null }, true)]
    [InlineData("This is a test string.", new object[] { "ring.", "", "abc" }, true)]
    [InlineData("Hello World!", new object[] { "elo", "ABC", "" }, false)]
    [InlineData("", new object[] { "o", "d", "World!" }, false)]
    public void EndsWithAnyObjectTest(string str, IEnumerable<object> array, bool expectedResult)
    {
        var result = str.EndsWithAny(array);
        Assert.Equal(expectedResult, result);

        result = str.EndsWithAny(array.ToArray());
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData("Hello World!", new string[] { "o", "d", "World!" }, true)]
    [InlineData("This is a test string.", new string[] { "ring.", "", "abc" }, true)]
    [InlineData("Hello World!", new string[] { "ell", "ABC", "" }, false)]
    [InlineData("", new string[] { "o", "d", "World!" }, false)]
    public void EndsWithAnyStringTest(string str, IEnumerable<string> values, bool expectedResult)
    {
        var result = str.EndsWithAny(values);
        Assert.Equal(expectedResult, result);

        result = str.EndsWithAny(values.ToArray());
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData("hello", "hello", true, true)]
    [InlineData("hello", "hello", false, true)]
    [InlineData("hello", "heLLo", true, true)]
    [InlineData("hello", "heLLo", false, false)]
    public void EqualsTo(string str1, string str2, bool ignoreCase, bool expected)
    {
        var actual = StringHelper.EqualsTo(str1, str2, ignoreCase);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("foo", "foo", "bar", "baz")]
    [InlineData("bar", "foo", "bar", "baz")]
    [InlineData("baz", "foo", "bar", "baz")]
    public void EqualsToAny_Should_Return_True_For_Expected_Values(string input, string expected1, string expected2, string expected3)
    {
        // Arrange
        var array = new string[] { expected1, expected2, expected3 };

        // Act
        var result = input.EqualsToAny(array);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(null, 5, ' ', "     ")]
    [InlineData("", 5, ' ', "     ")]
    [InlineData("test", 5, ' ', "test ")]
    [InlineData("testtest", 5, ' ', "testt")]
    [InlineData("test", 0, ' ', "")]
    [InlineData("test", 5, '-', "test-")]
    [InlineData("testtest", 5, '-', "testt")]
    [InlineData("test", 0, '-', "")]
    public void FixSize_ReturnsCorrectValue(string? str, int maxLength, char gapChar, string? expected)
    {
        // Act
        var actual = StringHelper.FixSize(str, maxLength, gapChar);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("Hello ", new[] { "World" }, "Hello ")]
    [InlineData("Hello {0}", new[] { "World" }, "Hello World")]
    [InlineData("Hello {0}", new object[] { "World", "!" }, "Hello World")]
    [InlineData("{0} + {1} = {2}", new object[] { 2, 3, 5 }, "2 + 3 = 5")]
    public void Format_ShouldReturnFormattedString(string format, object[] args, string expected)
    {
        var result = format.Format(args);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Format_ShouldThrowArgumentNullException_WhenFormatIsNullEmpty()
    {
        string? format = null;
        _ = Assert.Throws<ArgumentNullException>(() => format.Format(null));
    }

    [Fact]
    public void Format_ShouldThrowArgumentNullException_WhenFormatIsNullEmptyButArgsIsNotNull()
    {
        // Arrange
        string? format = null;
        object[] args = { 1, "foo" };

        // Act & Assert
        _ = Assert.Throws<ArgumentNullException>(() => format.Format(args));
    }

    [Fact]
    public void Format_ShouldThrowFormatException_WhenFormatIsInvalid()
    {
        // Arrange
        var format = "{0} {1}"; // Missing argument for {1}
        object[] args = { "foo" };

        // Act & Assert
        _ = Assert.Throws<FormatException>(() => format.Format(args));
    }

    [Theory]
    [InlineData(LONG_TEXT, 0, '\'', '\'', "a text")]
    [InlineData(LONG_TEXT, 0, '\'', NULL_CHAR, "a text")]
    [InlineData(LONG_TEXT, 1, '\'', '\'', "another text")]
    [InlineData(LONG_TEXT, 1, 'q', NULL_CHAR, null)]
    public void GetPhrase(string? str, int index, char start, char end, string expected)
    {
        var actual = str.GetPhrase(index, start, end);
        Assert.Equal(expected, actual);
    }

    [Trait("Category", "Optimizations")]
    [Theory(DisplayName = $"{nameof(GetPhrase_StringOptimization)} : memory allocation.")]
    [InlineData(@"https://my.site.com", "://", "/", "my.site.com")]
    [InlineData(@"https://my.site.com/", "://", "/", "my.site.com")]
    [InlineData(@"https://my.site.com/clientarea.php", "://", "/", "my.site.com")]
    [InlineData(@"https://my.site.com/clientarea.php?id={50E204C7-C1BC-4139-A339-100090D3E493}", "://", "/", "my.site.com")]
    [InlineData(@"This is mohammad", "mohammad", "hello", "")]
    [InlineData(@"This is mohammad", "", "hello", "This is mohammad")]
    [InlineData(@"This is mohammad", "", "", "")]
    public void GetPhrase_StringOptimization(string text, string start, string end, string expected)
    {
        var actual = StringHelper.GetPhrase(text, start, end);
        Assert.Equal(expected, actual);
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
    public void GroupByTest()
    {
        // Arrange
        var chars = new List<char> { 'H', 'e', 'l', 'l', 'o', ',', 'W', 'o', 'r', 'l', 'd', ',', 'H', 'e', 'l', 'l', 'o' };
        var separator = ',';

        // Act
        var result = chars.GroupBy(separator);

        // Assert
        Assert.Collection(result,
            r1 => Assert.Equal("Hello", r1),
            r2 => Assert.Equal("World", r2));
    }

    [Fact]
    public void HasPersian()
    {
        var hasPersian = "Hello. My اسم is Mohammad.";
        var has = StringHelper.HasPersian(hasPersian);
        Assert.True(has);

        var hasNotPersian = "Hello. My name is Mohammad";
        var hasNot = StringHelper.HasPersian(hasNotPersian);
        Assert.True(hasNot);
    }

    [Theory]
    [InlineData(null, "default")]
    [InlineData("", "default")]
    [InlineData("test", "test")]
    public void IfNullOrEmptyTest(string str, string defaultValue)
    {
        var result = str.IfNullOrEmpty(defaultValue);

        Assert.Equal(defaultValue, result);
    }

    [Theory]
    [InlineData(null, "default value", "default value")]
    [InlineData("", "default value", "")]
    [InlineData("not null", "default value", "not null")]
    [InlineData(" ", "default value", " ")]
    public void IfNullTest1(string? s, string? value, string? expected)
    {
        // Act
        var result = s.IfNull(value);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(IfNullData))]
    public void IfNullTest2(string? s, string? value, string? expected)
    {
        // Act
        var result = s.IfNull(value);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(null, true)]
    [InlineData("", true)]
    [InlineData("Hello", false)]
    public void IsNullOrEmptyTest(string str, bool expected)
    {
        // Arrange Nothing to do

        // Act
        var actual = str.IsNullOrEmpty();

        // Assert
        Assert.Equal(expected, actual);
    }

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
    [InlineData("numbers", "number")]
    [InlineData("cases", "case")]
    [InlineData("handies", "handy")]
    [InlineData("people", "person")]
    [InlineData("children", "child")]
    public void PluralizeTest(string pluralized, string single)
        => Assert.Equal(pluralized, StringHelper.Pluralize(single));

    [Theory]
    [InlineData("hello world", "world", "hello ")]
    [InlineData("hello world", "not_exist", "hello world")]
    [InlineData(null, "value", null)]
    [InlineData("value", null, "value")]
    [InlineData(null, null, null)]
    public void RemoveTest(string? str, string? value, string? expected)
    {
        var result = str.Remove(value);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("hello", 3, "hellohellohello")]
    [InlineData("world", 1, "world")]
    public void RepeatTest(string text, int count, string expected)
    {
        var result = text.Repeat(count);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ReplaceAll_ShouldReplaceAllOccurrencesOfOldValuesWithNewValue()
    {
        // Arrange
        var value = "Hello world!";
        var oldValues = new List<string> { "o", "l" };
        var newValue = "*";

        // Act
        var result = value.ReplaceAll(oldValues, newValue);

        // Assert
        Assert.Equal("He*** w*r*d!", result);
    }

    [Theory]
    [InlineData("ThisIsMohammad", "This Is Mohammad")]
    [InlineData("Just_for_separate", "Just for separate")]
    [InlineData("This-is-another-sentence.", "This is another sentence.")]
    [InlineData("", "")]
    [InlineData(null, null)]
    public void SeparateCamelCaseTest(string text, string expected)
    {
        var actual = text.Separate();
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("numbers", "number")]
    [InlineData("cases", "case")]
    [InlineData("handies", "handy")]
    [InlineData("people", "person")]
    [InlineData("children", "child")]
    public void SingularizeTest(string pluralized, string single)
        => Assert.Equal(single, StringHelper.Singularize(pluralized));

    [Fact]
    public void Slice()
    {
        var sliceStr = "Mohammad Mirmostafa is a C# Developer.";

        var mohammad = sliceStr.Slice(0, 8);
        Assert.Equal("Mohammad", mohammad);

        var cSharpDeveloper = sliceStr.Slice(25);
        Assert.Equal("C# Developer.", cSharpDeveloper);
    }

    [Fact]
    public void Space()
        => Assert.Equal("     ", StringHelper.Space(5));

    [Fact]
    public void SpaceException()
        => Assert.Throws<ArgumentOutOfRangeException>(() => Assert.Equal("     ", StringHelper.Space(-5)));

    [Theory]
    [InlineData("Hello, my name is Mohammad. How are you? Just fine. @mohammad.", new[] { "Hello, my name is ", "Mohammad. ", "How are you? ", "Just fine. @mohammad." })]
    public void SplitMergeTest(string text, string[] expected)
    {
        var actual = text.SplitCamelCase();
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(@"Server=myServerAddress;Database=MyDatabase;Trusted_Connection=True;", "Database", "MyDatabase")]
    public void SplitPair(string text, string slice, string expected)
    {
        var pairs = StringHelper.SplitPair(text);
        (_, var result) = pairs.Where(kv => kv.Key == slice).Single();
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("Hello", "hello", true, true)]
    [InlineData("Hello", "hello", false, false)]
    [InlineData("Hello", "Hello", true, true)]
    [InlineData("Hello", "World", true, false)]
    public void TestEqualsTo(string str1, string str2, bool ignoreCase, bool expectedResult)
    {
        var result = str1.EqualsTo(str2, ignoreCase);
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData("Hello", true, new string[] { "hello", "world", "HELLO" }, true)]
    [InlineData("Hello", false, new string[] { "hello", "world", "HELLO" }, false)]
    [InlineData("Hello", true, new string[] { "HELLO", "world", "hi" }, true)]
    [InlineData("Hello", true, new string[] { "world", "hi" }, false)]
    public void TestEqualsToAny(string str1, bool ignoreCase, string[] array, bool expectedResult)
    {
        var result = str1.EqualsToAny(ignoreCase, array);
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData("0062614614", true)] // Valid code
    [InlineData("0062614615", false)] // Invalid code
    [InlineData("0013542419", true)] // Valid code
    [InlineData("1234567890", false)] // Invalid code
    [InlineData("0499370891", false)] // Invalid code
    [InlineData("04993A0891", false)] // Invalid code
    [InlineData("", false)] // Invalid code
    public void TestIsValidIranianNationalCode(string nationalCode, bool expected)
    {
        var actual = StringHelper.IsValidIranianNationalCode(nationalCode);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("Hello world", "world", "Bing", 1, "Hello Bing")] // Replace once
    [InlineData("Hello world", "l", "*", 2, "He**o world")] // Replace twice
    [InlineData("Hello world", "o", "#", -1, "Hell# w#rld")] // Replace all
    public void TestReplace2(string input, string old, string replacement, int count, string expected) =>
        // Call the static method and assert the result
        Assert.Equal(expected, input.Replace2(old, replacement, count));

    [Fact]
    public void ToLower_ReturnsLowercaseStrings()
    {
        // Arrange
        IEnumerable<string> strings = new List<string> { "FOO", "Bar", "BaZ" };

        // Act
        var result = strings.ToLower();

        // Assert
        Assert.Collection(result,
            item => Assert.Equal("foo", item),
            item => Assert.Equal("bar", item),
            item => Assert.Equal("baz", item));
    }

    [Fact]
    public void ToUnicode_ShouldReturnConvertedString_WhenStringIsNonAscii()
    {
        // Arrange
        var str = "سلام";

        // Act
        var result = str.ToUnicode();

        // Assert
        Assert.Equal("\u0633\u0644\u0627\u0645", result);
    }

    [Fact]
    public void ToUnicode_ShouldReturnNull_WhenStringIsNull()
    {
        // Arrange
        string? str = null;

        // Act
        var result = str.ToUnicode();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ToUnicode_ShouldReturnSameString_WhenStringIsAscii()
    {
        // Arrange
        var str = "Hello";

        // Act
        var result = str.ToUnicode();

        // Assert
        Assert.Equal(str, result);
    }

    [Fact]
    public void TrimAll_ShouldReturnEmptySequence_WhenValuesIsEmpty()
    {
        // Arrange
        var values = Enumerable.Empty<string>();

        // Act
        var result = values.TrimAll();

        // Assert
        Assert.Empty(result);
    }

    [Theory]
    [InlineData(new[] { " a ", " b ", " c " }, new[] { "a", "b", "c" })]
    [InlineData(new[] { "", " ", "\t" }, new[] { "", "", "" })]
    public void TrimAll_ShouldTrimAllStrings(string[] input, string[] expected)
    {
        // Arrange
        var values = input as IEnumerable<string>;

        // Act
        var result = values.TrimAll();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TrimAll_ShouldTrimSpecifiedChars_WhenTrimCharsIsProvided()
    {
        // Arrange
        IEnumerable<string> values = new[] { "*Hello*", "*World*", "*" };

        // Act
        var result = values.TrimAll('*');

        // Assert
        Assert.Equal(new[] { "Hello", "World", "" }, result);
    }

    [Fact]
    public void TrimAll_ShouldTrimWhiteSpaceByDefault_WhenTrimCharsIsNotProvided()
    {
        // Arrange
        IEnumerable<string> values = new[] { " Hello ", " World ", "!" };

        // Act
        var result = values.TrimAll();

        // Assert
        Assert.Equal(new[] { "Hello", "World", "!" }, result);
    }
}

file class CompactDataClass : IEnumerable<object[]>
{
    public static object[][] CompactData => new[]
        {
        new object[]
        {
            new[] { "Hello", "", "I ", "am", "", "Mohammad", ".", "", "I'm", "", "a ", "", "C# ", "", "Developer." },
            new[] { "Hello", "I ", "am", "Mohammad", ".", "I'm", "a ", "C# ", "Developer." }
        }
    };

    public IEnumerator<object[]> GetEnumerator() => ((IEnumerable<object[]>)CompactData).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => CompactData.GetEnumerator();
}