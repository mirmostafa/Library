namespace UnitTests;

[Trait("Category", "Helpers")]
public class EnumerableHelperTest
{
    private readonly int[] _age = new[] { 10, 20, 30, 40, 50, 60 };
    private readonly string[] _names = new[] { "Nick", "Mike", "John", "Leyla", "David", "Damian" };

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

    [Fact(Skip = "Not required in .Net 6.0")]
    [Obsolete]
    public void ChunkByTest()
    {
        var chunks = this._names.ChunkBy(3);
        Assert.NotNull(chunks);
        Assert.Equal(2, chunks.Count());
        Assert.Equal(3, chunks.ElementAt(0).Count());
        Assert.Equal(3, chunks.ElementAt(1).Count());
        Assert.Equal("Leyla", chunks.ElementAt(1).ElementAt(0));
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
    public void EqualTest(IEnumerable<int> ints1, IEnumerable<int> ints2,bool ignoreIndexes, bool expected)
    {
        var actual = EnumerableHelper.Equal(ints1, ints2, ignoreIndexes);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CountNotEnumeratedTest()
        => Assert.Equal(6, this._names.CountNotEnumerated());
}