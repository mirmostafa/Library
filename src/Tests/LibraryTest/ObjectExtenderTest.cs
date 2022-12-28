namespace UnitTest;

[TestClass]
public class ObjectExtenderTest
{
    [TestMethod]
    public void MyTestMethod()
    {
        var testString = "Ali";
        testString.props().IsReadOnly = true;
        var actual = testString.props().IsReadOnly;
        Assert.IsTrue(actual);
    }
}