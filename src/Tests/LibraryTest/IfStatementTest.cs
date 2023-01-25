using Library.Coding;

namespace Library.UnitTest;

[TestClass]
public class IfStatementTest
{
    [TestMethod]
    public void IfStatementTestMethod1()
        => true.Fluent().If().Then(Methods.Empty).Else(Methods.Empty);

    [TestMethod]
    public void IfStatementTestMethod2()
    {
        var actual = true.Fluent().If<int>().Then(() => 1).Else(() => 2);
        Assert.AreEqual(1, actual);
    }

    [TestMethod]
    public void IfStatementTestMethod3()
    {
        var actual = false.Fluent().If<int>().Then(() => 1).Else(() => 2);
        Assert.AreEqual(2, actual);
    }

    [TestMethod]
    public void IfStatementTestMethod4()
    {
        var _true = () => true;
        _true.Fluent().If().Then(Methods.Empty).Else(Methods.Empty);
    }

    [TestMethod]
    public void IfStatementTestMethod5()
    {
        var _true = () => true;
        var actual = _true.Fluent().If<int>().Then(() => 1).Else(() => 2);
        Assert.AreEqual(1, actual);
    }

    [TestMethod]
    public void IfStatementTestMethod6()
    {
        var _false = () => false;
        var actual = _false.Fluent().If<int>().Then(() => 1).Else(() => 2);
        Assert.AreEqual(2, actual);
    }
}