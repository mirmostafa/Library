namespace UnitTests;

[Trait("Category", "Helpers")]
public class EnumerableHelperTest
{
    private readonly string[] _names = new[] { "Nick", "Mike", "John", "Leyla", "David", "Damian" };

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

    [Fact(Skip = "Not required in .Net ^6.0")]
    [Obsolete("Not required in .Net ^6.0")]
    public void ChunkByTest()
    {
        var chunks = this._names.ChunkBy(3);
        Assert.NotNull(chunks);
        Assert.Equal(2, chunks.Count());
        Assert.Equal(3, chunks.ElementAt(0).Count());
        Assert.Equal(3, chunks.ElementAt(1).Count());
        Assert.Equal("Leyla", chunks.ElementAt(1).ElementAt(0));
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

    [Theory]
    [InlineData(null, null, new int[0])]
    [InlineData(new[] {1,2,3}, null, new[] { 1, 2, 3 })]
    [InlineData(null, new[] { 1, 2, 3 }, new[] { 1, 2, 3 })]
    [InlineData(new[] { 1, 2, 3 }, new[] { 4, 5, 6 }, new[] { 1, 2, 3, 4, 5, 6 })]
    public void AddRangeImmutedTest(IEnumerable<int> source, IEnumerable<int> items, IEnumerable<int> expected)
    {
        var actual = EnumerableHelper.AddRangeImmuted(source, items);
        Assert.Equal(expected, actual);
    }
}