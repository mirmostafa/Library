namespace UnitTests;


public class EnumerableHelperTest
{
    private readonly string[] _names = new[] { "Nick", "Mike", "John", "Leyla", "David", "Damian" };
    private readonly int[] _age = new[] { 10, 20, 30, 40, 50, 60 };

    //[Fact]
    //public void AddRangeAsyncTest()
    //{
    //    var numberQuery = Enumerable.Range(1, 10).AsQueryable();
    //    var numbers = numberQuery.ToEnumerableAsync();
    //    var numberList = numbers.ToListAsync().Result;
    //    var result = numberList.AddRangeAsync(numbers).Result;
    //    Assert.Equal(10, result.Count);
    //}

    [Fact( Skip ="Not required in .Net 6.0")]
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
}
