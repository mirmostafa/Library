using System.Collections;
using System.Collections.ObjectModel;

namespace UnitTests;

[Trait("Category", nameof(Library.Helpers))]
[Trait("Category", nameof(EnumerableHelper))]
public sealed class EnumerableHelperTest
{
    private readonly string[] _names = ["Nick", "Mike", "John", "Leyla", "David", "Damian"];

    public static TheoryData<IEnumerable<object?>?, IEnumerable<object>> TestCompactData =>
        new()
        {
            { new string?[] { "a", "b", null, "c" }, ["a", "b", "c"] }, // Filter out a null value from a non-empty sequence
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
    public void AddRangeImmutedTest(IEnumerable<int>? source, IEnumerable<int>? items, IEnumerable<int> expected)
    {
        var actual = EnumerableHelper.AddRangeImmuted(source, items);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void AnyReturnsFalseWhenSourceIsEmpty()
    {
        // Arrange
        IEnumerable source = Array.Empty<int>();

        // Act
        var result = EnumerableHelper.Any(source);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void AnyReturnsFalseWhenSourceIsNull()
    {
        // Arrange
        IEnumerable? source = null;

        // Act
        var result = EnumerableHelper.Any(source);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void AnyReturnsTrueWhenSourceIsNotNullAndNotEmpty()
    {
        // Arrange
        IEnumerable source = new[] { 1, 2, 3 };

        // Act
        var result = EnumerableHelper.Any(source);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void BuildReturnsReadOnlyList()
    {
        // Arrange
        var items = new List<int> { 1, 2, 3 };

        // Act
        var result = items.Build();

        // Assert
        Assert.Equal(items.Count, result.Count);
    }

    [Theory]
    [InlineData(new[] { 1, 2, 3 })]
    public void CopyArrayWithElementsReturnsCopyOfArray<T>(T[] array)
    {
        // Arrange

        // Act
        var result = array.Copy();

        // Assert
        Assert.Equal(array, result);
        Assert.NotSame(array, result);
    }

    [Theory]
    [InlineData(new int[0])]
    public void CopyEmptyArrayReturnsEmptyArray<T>(T[] array)
    {
        // Arrange

        // Act
        var result = array.Copy();

        // Assert
        Assert.Empty(result);
    }

    [Theory]
    [InlineData(new string[] { "a", "b", "c" }, new string[] { "a", "b", "c" }, true)]
    [InlineData(new string[] { "a", "b", "c" }, new string[] { "a", "b" }, false)]
    [InlineData(new int[] { 10, 20, 30 }, new int[] { 10, 20, 30 }, true)]
    [InlineData(new int[] { 10, 20, 30 }, new int[] { 10, 20 }, false)]
    public void CopyStringArrayCompareCopiedArrays<T>(T[] source, T[] destination, bool expected)
    {
        // Arrange

        // Act
        var result = source.Copy();

        // Assert
        Assert.NotSame(source, result);
        if (expected)
        {
            Assert.Equal(destination, result);
        }
        else
        {
            Assert.NotEqual(destination, result);
        }
    }

    [Fact]
    public void CopyStringArrayReturnsCopyOfStringArray()
    {
        // Arrange
        var array = new[] { "foo", "bar", "baz" };

        // Act
        var result = array.Copy();

        // Assert
        Assert.Equal(array, result);
        Assert.NotSame(array, result);
    }

    [Fact]
    public void CountNotEnumeratedTest()
        => Assert.Equal(6, this._names.CountNotEnumerated());

    [Fact]
    public void EnumerateWithCancelledToken()
    {
        // Arrange
        IEnumerable<int> values = [1, 2, 3];
        static int action(int x) => x * 2;
        using var tokenSource = new CancellationTokenSource();
        tokenSource.Cancel();

        // Act
        var result = values.Enumerate(action, tokenSource.Token);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void EnumerateWithEmptyList()
    {
        // Arrange
        IEnumerable<int> values = [];
        static int action(int x) => x * 2;

        // Act
        var result = values.Enumerate(action);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void EnumerateWithNullValues()
    {
        // Arrange
        IEnumerable<int>? values = null;
        static int action(int x) => x * 2;

        // Act
        var result = values!.Enumerate(action);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void EnumerateWithValidInput()
    {
        // Arrange
        IEnumerable<int> values = [1, 2, 3];
        static int action(int x) => x * 2;

        // Act
        var result = values.Enumerate(action).ToList();

        // Assert
        Assert.Equal([2, 4, 6], result);
    }

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
    public void FindDuplicatesOnNonDistinctListTest()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3, 4, 2 };
        var expected = new List<int> { 2 };

        // Act
        var actual = list.FindDuplicates();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void IndexesOfReturnsCorrectIndexes()
    {
        // Arrange
        var items = new List<int> { 1, 2, 3, 2, 4, 2 };

        // Act
        var result = items.IndexesOf(2);

        // Assert
        Assert.Equal([1, 3, 5], result);
    }

    [Fact]
    public void IndexesOfReturnsCorrectIndexesForNullItem()
    {
        // Arrange
        var items = new List<string?> { "hello", null, "world", null, null };

        // Act
        var result = items.IndexesOf(null);

        // Assert
        Assert.Equal([1, 3, 4], result);
    }

    [Fact]
    public void IndexesOfReturnsEmptyForNonexistentItem()
    {
        // Arrange
        var items = new List<int> { 1, 2, 3, 4 };

        // Act
        var result = items.IndexesOf(5);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void IsSameReturnsFalseForDifferentCollections()
    {
        // Arrange
        var items1 = new List<int> { 1, 2, 3 };
        var items2 = new List<int> { 1, 2, 4 };

        // Act
        var result = items1.IsSame(items2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsSameReturnsFalseForOneNullCollection()
    {
        // Arrange
        var items1 = new List<int> { 1, 2, 3 };
        List<int>? items2 = null;

        // Act
        var result = items1.IsSame(items2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsSameReturnsTrueForEqualCollections()
    {
        // Arrange
        var items1 = new List<int> { 1, 2, 3 };
        var items2 = new List<int> { 1, 2, 3 };

        // Act
        var result = items1.IsSame(items2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsSameReturnsTrueForNullCollections()
    {
        // Arrange
        List<int>? items1 = null;
        List<int>? items2 = null;

        // Act
        var result = items1.IsSame(items2);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(new[] { 1, 2, 3, 4, 5 }, -1, 5, new[] { 1, 2, 3, 4 })]
    [InlineData(new[] { 1, 2, 3, 4, 5 }, -3, 3, new[] { 1, 2, 4, 5 })]
    [InlineData(new[] { "A", "B", "C" }, -2, "B", new[] { "A", "C" })]
    public void PopRemovesAndReturnsElementAtSpecifiedIndexNegativeIndex<T>(T[] input, int index, T expected, T[] remaining)
    {
        // Arrange
        var list = new List<T>(input);

        // Act
        var result = list.Pop(index);

        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(remaining.Length, list.Count);
        Assert.Equal(remaining, list);
    }

    [Theory]
    [InlineData(new[] { 1, 2, 3, 4, 5 }, 2, 3, new[] { 1, 2, 4, 5 })]
    [InlineData(new[] { "A", "B", "C" }, 1, "B", new[] { "A", "C" })]
    public void PopRemovesAndReturnsElementAtSpecifiedIndexPositiveIndex<T>(T[] input, int index, T expected, T[] remaining)
    {
        // Arrange
        var list = new List<T>(input);

        // Act
        var result = list.Pop(index);

        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(remaining.Length, list.Count);
        Assert.Equal(remaining, list);
    }

    [Theory]
    [InlineData(new[] { 1, 2, 3, 4, 5 }, 5, new[] { 1, 2, 3, 4 })]
    [InlineData(new[] { "A", "B", "C" }, "C", new[] { "A", "B" })]
    public void PopRemovesAndReturnsLastElement<T>(T[] input, T expected, T[] remaining)
    {
        // Arrange
        var list = new List<T>(input);

        // Act
        var result = list.Pop();

        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(remaining.Length, list.Count);
        Assert.Equal(remaining, list);
    }

    [Theory]
    [InlineData(new[] { 1, 2, 3, 4, 5 }, 10)]
    [InlineData(new[] { "A", "B", "C" }, 5)]
    public void PopRemovesAndReturnsLastElementWhenIndexIsGreaterThanListLength<T>(T[] input, int index)
    {
        // Arrange
        var list = new List<T>(input);

        // Act
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => list.Pop(index));
    }

    [Fact]
    public void RemoveDefaultsOnListWithDefault()
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
    public void RemoveDefaultsOnListWithNull()
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
    public void RemoveDefaultsWithCustomValue()
    {
        // Arrange
        var list = new List<int> { 1, 2, -1, 3 };
        var expected = new List<int> { 1, 2, 3 };

        // Act
        var actual = list.RemoveDefaults(-1);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("John", 5)]
    [InlineData(3000, 2999)]
    public void RepeatTest(object item, int count)
    {
        var actual = EnumerableHelper.Repeat(item, count).ToArray();
        for (var i = 0; i < count; i++)
        {
            Assert.Equal(item, actual[i]);
        }
    }

    [Fact]
    public void RunAllWhileTest()
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

    [Fact]
    public void SliceEndIndex()
    {
        // Assign
        var myFamily = new[] { "Dad", "Mom", "Me", "Sister 1", "Sister 2", "Sister 3" };
        var expected = new[] { "Dad", "Mom", "Me" };

        // Act
        var actual = myFamily.Slice(end: 3);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void SliceFull()
    {
        // Assign
        var myFamily = new[] { "Dad", "Mom", "Me", "Sister 1", "Sister 2", "Sister 3" };
        var expected = new[] { "Mom", "Sister 1" };

        // Act
        var actual = myFamily.Slice(1, 3, 2);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void SliceFullCopyArray()
    {
        // Assign
        var myFamily = new[] { "Dad", "Mom", "Me", "Sister 1", "Sister 2", "Sister 3" };
        var expected = new[] { "Dad", "Mom", "Me", "Sister 1", "Sister 2", "Sister 3" };

        // Act
        var actual = myFamily.Slice();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void SliceReverseArray()
    {
        // Assign
        var myFamily = new[] { "Dad", "Mom", "Me", "Sister 1", "Sister 2", "Sister 3" };
        var expected = new[] { "Sister 3", "Sister 2", "Sister 1", "Me", "Mom", "Dad" };

        // Act
        var actual = myFamily.Slice(steps: -1);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void SliceStartIndex()
    {
        // Assign
        var myFamily = new[] { "Dad", "Mom", "Me", "Sister 1", "Sister 2", "Sister 3" };
        var expected = new[] { "Sister 1", "Sister 2", "Sister 3" };

        // Act
        var actual = myFamily.Slice(start: 3);

        // Assert
        Assert.Equal(expected, actual);
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
        Span<int> span = [.. list];
        // Call the static method and assert the result
        Assert.Equal(expected, span);
    }

    [Theory]
    [MemberData(nameof(TestCompactData))]
    public void TestCompact(IEnumerable<object?>? input, IEnumerable<object> expected)
        => Assert.Equal(expected, input.Compact());

    [Fact]
    public async Task TestEnumerateAsyncWithCancelledToken()
    {
        // Arrange
        IEnumerable<int> values = [1, 2, 3];
        static async Task action(int x, CancellationToken token) => await Task.Delay(1000, token).ConfigureAwait(false);
        var tokenSource = new CancellationTokenSource();
        tokenSource.Cancel();

        // Act
        await values.EnumerateAsync(action, tokenSource.Token).ConfigureAwait(false);

        // Assert No assertions needed here because the method does not return anything.
    }

    [Fact]
    public async Task TestEnumerateAsyncWithEmptyList()
    {
        // Arrange
        IEnumerable<int> values = [];
        static async Task action(int x, CancellationToken token) => await Task.Delay(1000, token).ConfigureAwait(false);

        // Act
        await values.EnumerateAsync(action).ConfigureAwait(false);

        // Assert No assertions needed here because the method does not return anything.
    }

    [Fact]
    public async Task TestEnumerateAsyncWithNullValues()
    {
        // Arrange
        IEnumerable<int>? values = null;
        static async Task action(int x, CancellationToken token) => await Task.Delay(1000, token).ConfigureAwait(false);

        // Act
        await values.EnumerateAsync(action).ConfigureAwait(false);

        // Assert No assertions needed here because the method does not return anything.
    }

    [Fact]
    public async Task TestEnumerateAsyncWithValidInput()
    {
        // Arrange
        IEnumerable<int> values = [1, 2, 3];
        static async Task action(int x, CancellationToken token) => await Task.Delay(1000, token).ConfigureAwait(false);

        // Act
        await values.EnumerateAsync(action).ConfigureAwait(false);

        // Assert No assertions needed here because the method does not return anything.
    }

    [Fact]
    public void TestEnumerateWithCancelledToken()
    {
        // Arrange
        IEnumerable<int> values = [1, 2, 3];
        static void action(int x) => Console.WriteLine(x * 2);
        var tokenSource = new CancellationTokenSource();
        tokenSource.Cancel();

        // Act
        values.Enumerate(action, tokenSource.Token);

        // Assert No assertions needed here because the method does not return anything.
    }

    [Fact]
    public void TestEnumerateWithEmptyList()
    {
        // Arrange
        IEnumerable<int> values = [];
        static void action(int x) => Console.WriteLine(x * 2);

        // Act
        values.Enumerate(action);

        // Assert No assertions needed here because the method does not return anything.
    }

    [Fact]
    public void TestEnumerateWithNullValues()
    {
        // Arrange
        IEnumerable<int>? values = null;
        static void action(int x) => Console.WriteLine(x * 2);

        // Act
        values.Enumerate(action);

        // Assert No assertions needed here because the method does not return anything.
    }

    [Fact]
    public void TestEnumerateWithValidInput()
    {
        // Arrange
        IEnumerable<int> values = [1, 2, 3];
        static void action(int x) => Console.WriteLine(x * 2);

        // Act
        values.Enumerate(action);

        // Assert No assertions needed here because the method does not return anything.
    }

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

    [Fact]
    public void AddRange_EmptyCollection_ReturnsSameObservableCollection()
    {
        // Arrange
        var list = new ObservableCollection<int>();
        var emptyCollection = Enumerable.Empty<int>();

        // Act
        var result = list.AddRange(emptyCollection);

        // Assert
        Assert.Same(list, result);
        Assert.Empty(list);
    }

    [Fact]
    public void AddRange_NonEmptyCollection_AddsItemsToObservableCollection()
    {
        // Arrange
        var list = new ObservableCollection<string>();
        var itemsToAdd = new List<string> { "apple", "banana", "cherry" };

        // Act
        var result = list.AddRange(itemsToAdd);

        // Assert
        Assert.Same(list, result);
        Assert.Equal(3, list.Count);
        Assert.Contains("apple", list);
        Assert.Contains("banana", list);
        Assert.Contains("cherry", list);
    }

    [Fact]
    public void AddRange_ExistingList_AddsItemsToExistingObservableCollection()
    {
        // Arrange
        var existingList = new ObservableCollection<int> { 1, 2, 3 };
        var itemsToAdd = new List<int> { 4, 5 };

        // Act
        var result = existingList.AddRange(itemsToAdd);

        // Assert
        Assert.Same(existingList, result);
        Assert.Equal(5, existingList.Count);
        Assert.Contains(4, existingList);
        Assert.Contains(5, existingList);
    }

    [Fact]
    public void AddRange_EmptyCollection_ReturnsSameList()
    {
        // Arrange
        var list = new List<int>();
        var emptyCollection = Enumerable.Empty<int>();

        // Act
        var result = EnumerableHelper.AddRange(list, emptyCollection);

        // Assert
        Assert.Same(list, result);
        Assert.Empty(list);
    }
    [Fact]
    public void AddRange_NonEmptyCollection_AddsItemsToList2()
    {
        // Arrange
        var list = new List<string>();
        var itemsToAdd = new List<string> { "apple", "banana", "cherry" };

        // Act
        var result = EnumerableHelper.AddRange(list, itemsToAdd);

        // Assert
        Assert.Same(list, result);
        Assert.Equal(3, list.Count);
        Assert.Contains("apple", list);
        Assert.Contains("banana", list);
        Assert.Contains("cherry", list);
    }
    
    [Fact]
    public void AddRange_ExistingList_AddsItemsToExistingList()
    {
        // Arrange
        var existingList = new List<int> { 1, 2, 3 };
        var itemsToAdd = new List<int> { 4, 5 };

        // Act
        var result = EnumerableHelper.AddRange(existingList, itemsToAdd);

        // Assert
        Assert.Same(existingList, result);
        Assert.Equal(5, existingList.Count);
        Assert.Contains(4, existingList);
        Assert.Contains(5, existingList);
    }

    [Fact]
    public void Aggregate_EmptyCollection_ReturnsDefaultValue()
    {
        // Arrange
        var emptyCollection = Enumerable.Empty<int>();
        var defaultValue = 0; // Example default value

        // Act
        var result = emptyCollection.Aggregate((a, b) => a + b, defaultValue);

        // Assert
        Assert.Equal(defaultValue, result);
    }

    [Fact]
    public void Aggregate_SingleItemCollection_ReturnsSameItem()
    {
        // Arrange
        var singleItemCollection = new List<int> { 42 }; // Example single item
        var defaultValue = 0; // Example default value

        // Act
        var result = singleItemCollection.Aggregate((a, b) => a + b, defaultValue);

        // Assert
        Assert.Equal(singleItemCollection[0], result);
    }
    
    [Fact]
    public void Aggregate_MultipleItemsCollection_AppliesAggregatorRecursively()
    {
        // Arrange
        var multipleItemsCollection = new List<int> { 1, 2, 3, 4, 5 }; // Example multiple items
        var defaultValue = 0; // Example default value

        // Act
        var result = multipleItemsCollection.Aggregate((a, b) => a + b, defaultValue); // Example aggregator function

        // Assert
        Assert.Equal(15, result); // Expected result: 1 + 2 + 3 + 4 + 5
    }
    [Fact]
    public void ClearAndAdd_EmptyList_AddsItemToList()
    {
        // Arrange
        var emptyList = new List<int>();
        var itemToAdd = 42; // Example item

        // Act
        var result = emptyList.ClearAndAdd(itemToAdd);

        // Assert
        Assert.Same(emptyList, result);
        Assert.Single(emptyList);
        Assert.Equal(itemToAdd, emptyList[0]);
    }
    [Fact]
    public void ClearAndAdd_ExistingList_AddsItemToExistingList()
    {
        // Arrange
        var existingList = new List<string> { "apple", "banana" };
        var itemToAdd = "cherry"; // Example item

        // Act
        var actual = existingList.ClearAndAdd(itemToAdd);

        var expected = new[] { itemToAdd };

        // Assert
        Assert.Same(expected, actual);
        Assert.Equal(3, existingList.Count);
        Assert.Contains(itemToAdd, existingList);
    }
    [Fact]
    public void ClearAndAdd_DefaultValueList_AddsItemToDefaultList()
    {
        // Arrange
        var defaultValueList = new List<double> { 1.5, 2.5 };
        var itemToAdd = 3.0; // Example item

        // Act
        var result = defaultValueList.ClearAndAdd(itemToAdd);

        // Assert
        Assert.Same(defaultValueList, result);
        Assert.Equal(1, defaultValueList.Count);
        Assert.Contains(itemToAdd, defaultValueList);
    }
}