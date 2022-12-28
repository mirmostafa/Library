namespace UnitTest;

[TestClass]
public class ObjectHelperTest
{
    [TestMethod]
    public void ExtrapropertyTest()
    {
        var testString = "Ali";
        testString.props().IsReadOnly = true;
        var actual = testString.props().IsReadOnly;
        Assert.IsTrue(actual);
    }
}