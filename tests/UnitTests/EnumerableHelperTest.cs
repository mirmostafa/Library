using System.Collections;

namespace UnitTests;

[Trait("Category", "Helpers")]
public sealed class EnumerableHelperTest
{
    private readonly string[] _names = new[] { "Nick", "Mike", "John", "Leyla", "David", "Damian" };

    public static TheoryData<IEnumerable<object?>?, IEnumerable<object>> TestCompactData =>
        new()
        {
            { new string?[] { "a", "b", null, "c" }, new string[] { "a", "b", "c" } }, // Filter out a null value from a non-empty sequence
            { new string?[] { null, null, null }, Array.Empty<string>() }, // Filter out all null values from a non-empty sequence
            { Array.Empty<string?>(), Array.Empty<string>() }, // Filter out nothing from an empty sequence
            { null, Array.Empty<string>() } // Return an empty sequence for a null input
        };

    [Fact]
    public void AddImmutedToKeyValuePairTest()
    {
        var dic = new List<int> { 0, 1, 2, 3, 4, };
        var dic1 = dic.AddImmuted(5);
        dic1 = dic1.AddImmuted(6);

        var expected_dic_count = 5;
        var expected_dic1_count = 7;

        Assert.Equal(expected_dic_count, dic.Count);
        Assert.Equal(expected_dic1_count, dic1.Count);
    }

    [Theory]
    [InlineData(null, null, new int[0])]
    [InlineData(new[] { 1, 2, 3 }, null, new[] { 1, 2, 3 })]
    [InlineData(null, new[] { 1, 2, 3 }, new[] { 1, 2, 3 })]
    [InlineData(new[] { 1, 2, 3 }, new[] { 4, 5, 6 }, new[] { 1, 2, 3, 4, 5, 6 })]
    public void AddRangeImmutedTest(IEnumerable<int> source, IEnumerable<int> items, IEnumerable<int> expected)
    {
        var actual = EnumerableHelper.AddRangeImmuted(source, items);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void AddToKeyValuePairTest()
    {
        var dic = new List<KeyValuePair<int, string>>
        {
            { 0, "Zero" },
            { 1, "One"},
            { 2, "Two"},
            { 3, "Three" },
            { 4, "Four" }
        };
        _ = dic.Add(5, "Fix");
        _ = dic.Add(6, "Six");
        var expected = 7;
        var actual = dic.Count;
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void AggregateTest()
    {
        var first = new[] { 1, 2, 3 };
        var second = new[] { 4, 5, 6 };
        var merged = new[] { first, second };
        var actual = merged.SelectManyAndCompact();
        var expected = new[] { 1, 2, 3, 4, 5, 6 };
        Assert.True(expected.SequenceEqual(actual));
    }

    [Fact]
    public void Any_ReturnsFalse_WhenSourceIsEmpty()
    {
        // Arrange
        IEnumerable source = Array.Empty<int>();

        // Act
        var result = EnumerableHelper.Any(source);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Any_ReturnsFalse_WhenSourceIsNull()
    {
        // Arrange
        IEnumerable? source = null;

        // Act
        var result = EnumerableHelper.Any(source);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Any_ReturnsTrue_WhenSourceIsNotNullAndNotEmpty()
    {
        // Arrange
        IEnumerable source = new[] { 1, 2, 3 };

        // Act
        var result = EnumerableHelper.Any(source);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Build_ReturnsReadOnlyList()
    {
        // Arrange
        var items = new List<int> { 1, 2, 3 };

        // Act
        var result = items.Build();

        // Assert
        Assert.Equal(items.Count, result.Count);
    }

    [Fact]
    public void CountNotEnumeratedTest()
        => Assert.Equal(6, this._names.CountNotEnumerated());

    [Theory]
    [InlineData(new[] { 1, 2, 3 }, new[] { 1, 2, 3 }, false, true)]
    [InlineData(new[] { 1, 2, 3 }, new[] { 1, 2, 3 }, true, true)]

    [InlineData(new[] { 1, 3, 2 }, new[] { 1, 2, 3 }, true, true)]
    [InlineData(new[] { 1, 3, 2 }, new[] { 1, 2, 3 }, false, false)]

    [InlineData(new[] { 1, 2, 3 }, new[] { 1, 2, 4 }, true, false)]
    [InlineData(new[] { 1, 2, 3 }, new[] { 1, 2, 4 }, false, false)]

    [InlineData(new[] { 1, 2 }, new[] { 1, 2, 3 }, true, false)]
    [InlineData(new[] { 1, 2 }, new[] { 1, 2, 3 }, false, false)]

    [InlineData(new[] { 1, 2, 3 }, new[] { 1, 2 }, true, false)]
    [InlineData(new[] { 1, 2, 3 }, new[] { 1, 2 }, false, false)]
    public void EqualTest(IEnumerable<int> ints1, IEnumerable<int> ints2, bool ignoreIndexes, bool expected)
    {
        var actual = EnumerableHelper.Equal(ints1, ints2, ignoreIndexes);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FindDuplicatesOnDistinctListTest()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3, 4 };
        var expected = new List<int>();

        // Act
        var actual = list.FindDuplicates();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FindDuplicatesOnNonDistinctList_Test()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3, 4, 2 };
        var expected = new List<int> { 2 };

        // Act
        var actual = list.FindDuplicates();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, 10, 1, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })]
    [InlineData(10, 0, -1, new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 })]
    public void Range_ReturnsExpectedValues(int start, int end, int step, int[] expectedValues)
    {
        var actualValues = EnumerableHelper.Range(start, end, step).ToArray();
        Assert.Equal(expectedValues, actualValues);
    }

    [Fact]
    public void Range_ThrowsArgumentException_ForNegativeStepAndStartSmallerThanEnd()
        => Assert.Throws<Library.Exceptions.InvalidArgumentException>(() => EnumerableHelper.Range(0, 10, -1).ToArray());

    [Fact]
    public void Range_ThrowsArgumentException_ForPositiveStepAndStartLargerThanEnd()
        => Assert.Throws<Library.Exceptions.InvalidArgumentException>(() => EnumerableHelper.Range(10, 0, 1).ToArray());

    [Fact]
    public void Range_ThrowsArgumentOutOfRangeException_ForZeroStep()
        => Assert.Throws<ArgumentOutOfRangeException>(() => EnumerableHelper.Range(0, 10, 0).ToArray());

    [Theory]
    [InlineData(11, 2, new int[] { 0, 2, 4, 6, 8, 10 })]
    [InlineData(5, 1, new int[] { 0, 1, 2, 3, 4, 5 })]
    public void Range_WithEndOnly_ReturnsExpectedValues(int end, int step, int[] expectedValues)
    {
        var actualValues = EnumerableHelper.Range(end, step).ToArray();
        Assert.Equal(expectedValues, actualValues);
    }

    [Fact]
    public void Range_WithEndOnly_ThrowsArgumentException_ForNegativeStepAndStartSmallerThanEnd()
        => Assert.Throws<Library.Exceptions.InvalidArgumentException>(() => EnumerableHelper.Range(0, 10, -1).ToArray());

    [Fact]
    public void Range_WithEndOnly_ThrowsArgumentException_ForPositiveStepAndStartLargerThanEnd()
        => Assert.Throws<Library.Exceptions.InvalidArgumentException>(() => EnumerableHelper.Range(10, 0, 1).ToArray());

    [Fact]
    public void Range_WithEndOnly_ThrowsArgumentOutOfRangeException_ForZeroStep()
        => Assert.Throws<ArgumentOutOfRangeException>(() => EnumerableHelper.Range(0, 10, 0).ToArray());

    [Fact]
    public void RunAllWhile_Test()
    {
        // Arrange
        var actions = new List<Action>
        {
            () => Console.WriteLine("Action 1"),
            () => Console.WriteLine("Action 2"),
            () => Console.WriteLine("Action 3")
        };

        var counter = 0;
        bool predicate()
        {
            counter++;
            return counter < 2;
        }

        // Act
        actions.RunAllWhile(predicate);

        // Assert
        Assert.Equal(2, counter);
    }

    [Theory]
    [InlineData(new int[] { 1, 2, 3 }, 4, new int[] { 1, 2, 3, 4 })] // Add an item to a non-null array
    [InlineData(null, 5, new int[] { 5 })] // Add an item to a null array
    public void TestAddImmuted(int[]? input, int item, int[] expected) =>
        // Call the static method and assert the result
        Assert.Equal(expected, input.AddImmuted(item));

    [Theory]
    [InlineData(new int[] { 1, 2, 3 }, new int[] { 1, 2, 3 })] // Convert a non-empty list to a span
    [InlineData(new int[] { }, new int[] { })] // Convert an empty list to a span
    public void TestAsSpan(int[] input, int[] expected)
    {
        // Create a list from the input array
        var list = new List<int>(input);

        // Call the static method and assert the result
        Assert.Equal(expected, list.AsSpan().ToArray());
    }

    [Theory]
    [MemberData(nameof(TestCompactData))]
    public void TestCompact(IEnumerable<object?>? input, IEnumerable<object> expected)
        => Assert.Equal(expected, input.Compact());

    [Theory]
    [InlineData(3)]
    [InlineData(10)]
    [InlineData(0)]
    public void WithCancellationTest(int count)
    {
        // Assign
        using var cts = new CancellationTokenSource();
        var index = 0;

        // Act
        foreach (var name in this._names.WithCancellation(cts.Token))
        {
            if (index == count)
            {
                cts.Cancel();
            }
            else
            {
                index++;
            }
        }

        // Assert
        if (count > this._names.Length)
        {
            if (index != this._names.Length)
            {
                Assert.Fail($"Expected value was: {this._names.Length}. But actual value is: {index}");
            }
        }
        else
        {
            if (index != count)
            {
                Assert.Fail($"Expected value was: {count}. But actual value is: {index}");
            }
        }
    }

}

[Trait("Category", "Helpers")]
[Collection("RemoveDefaultsTest")]
public sealed class RemoveDefaultsTest
{
    [Fact]
    public void TestRemoveDefaultsOnListWithDefault()
    {
        // Arrange
        var list = new List<int> { 1, 2, 0, 3 };
        var expected = new List<int> { 1, 2, 3 };

        // Act
        var actual = list.RemoveDefaults();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestRemoveDefaultsOnListWithNull()
    {
        // Arrange
        var list = new List<string?> { "a", "b", null, "c" };
        var expected = new List<string?> { "a", "b", "c" };

        // Act
        var actual = list.RemoveDefaults();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestRemoveDefaultsWithCustomValue()
    {
        // Arrange
        var list = new List<int> { 1, 2, -1, 3 };
        var expected = new List<int> { 1, 2, 3 };

        // Act
        var actual = list.RemoveDefaults(-1);

        // Assert
        Assert.Equal(expected, actual);
    }
}