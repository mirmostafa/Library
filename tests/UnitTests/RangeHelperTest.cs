namespace UnitTests;


[Trait("Category", "Helpers")]
public sealed class RangeHelperTest
{
    [Theory]
    [InlineData(0, 10)]
    [InlineData(10, 0)]
    public void BasicTest(int min, int max)
    {
        foreach (var item in min..max)
        {
        }
    }
}