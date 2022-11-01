namespace UnitTest;

[TestClass]
public class RangeHelperTest
{
    [TestMethod]
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