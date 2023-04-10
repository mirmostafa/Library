namespace UnitTests;

[Trait("Category", "Helpers")]
public sealed class RangeHelperTest
{
    [Theory]
    [InlineData(0, 10, 55)]
    [InlineData(0, 1000, 500500)]
    [InlineData(10, 0, 0)]
    public void BasicTest(int min, int max, int expected)
    {
        var actual = 0;
        foreach (var item in min..max)
        {
            actual += item;
        }
        Assert.Equal(expected, actual);
    }
}