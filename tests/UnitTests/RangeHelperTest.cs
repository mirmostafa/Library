namespace UnitTests;


public class RangeHelperTest
{
    [Fact]
    public void BasicTest()
    {
        try
        {
            foreach (var item in 0..10)
            {
            }
        }
        catch (Exception ex)
        {
            Assert.Fail(ex.GetBaseException().Message);
        }
    }
}