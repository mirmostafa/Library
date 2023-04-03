namespace UnitTests;

[Trait("Category", "Helpers")]
public class ObjectHelperTest
{
    [Fact]
    public void ExtraPropertyTest()
    {
        var testString = "Ali";
        testString.props().IsReadOnly = true;
        var actual = testString.props().IsReadOnly;
        Assert.True(actual);
    }

    // TODO: Delete this code.
    //[Fact]
    //public void MapTest()
    //{
    //    var a = "5".Map(Convert.ToInt32);
    //    var b = 5.Map(Convert.ToString);
    //}
}