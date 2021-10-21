namespace LibraryTest;

[TestClass]
public class EnumerableHelperTest
{
    private readonly string[] names = new[] { "Nick", "Mike", "John", "Leyla", "David", "Damian" };
    private readonly int[] age = new[] { 10, 20, 30, 40, 50, 60 };

    //[TestMethod]
    public void AddRangeAsyncTest()
    {
        //var numberQuery = Enumerable.Range(1, 10).AsQueryable();
        //var numbers = numberQuery.AsEnumerableAsync();
        //var numberList = numbers.ToListAsync().Result;
        //var result = numberList.AddRangeAsync(numbers).Result;
        //Assert.AreEqual(10, result.Count);
    }

    [TestMethod]
    public void ChunkByTest()
    {
        var chunks = this.names.ChunkBy(3);
        Assert.IsNotNull(chunks);
        Assert.AreEqual(2, chunks.Count());
        Assert.AreEqual(3, chunks.ElementAt(0).Count());
        Assert.AreEqual(3, chunks.ElementAt(1).Count());
        Assert.AreEqual("Leyla", chunks.ElementAt(1).ElementAt(0));
    }

    [TestMethod]
    public void CountNotEnumeratedTest()
        => Assert.AreEqual(6, this.names.CountNotEnumerated());
}
