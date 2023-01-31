namespace UnitTests;


[Trait("Category", "Helpers")]
public class RangeHelperTest
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