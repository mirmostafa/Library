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
}