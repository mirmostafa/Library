namespace UnitTests;

[Trait("Category", "Helpers")]
public sealed class ObjectHelperTest
{
    [Fact]
    public void ExtraPropertyTest()
    {
        var testString = "Ali";
        var fluency = testString.Fluent();
        fluency.props().IsReadOnly = true;
        var actual = fluency.props().IsReadOnly;
        Assert.True(actual);
    }
}