using Library.Coding;

namespace UnitTests;


[Trait("Category", "Code Helpers")]
public sealed class IfStatementTest
{
    [Fact]
    public void IfStatementTestMethod1()
        => true.Fluent().If().Then(Methods.Empty).Else(Methods.Empty);

    [Fact]
    public void IfStatementTestMethod2()
    {
        var actual = true.Fluent().If<int>().Then(() => 1).Else(() => 2);
        Assert.Equal(1, actual);
    }

    [Fact]
    public void IfStatementTestMethod3()
    {
        var actual = false.Fluent().If<int>().Then(() => 1).Else(() => 2);
        Assert.Equal(2, actual);
    }

    [Fact]
    public void IfStatementTestMethod4()
    {
        var _true = () => true;
        _true.Fluent().If().Then(Methods.Empty).Else(Methods.Empty);
    }

    [Fact]
    public void IfStatementTestMethod5()
    {
        var _true = () => true;
        var actual = _true.Fluent().If<int>().Then(() => 1).Else(() => 2);
        Assert.Equal(1, actual);
    }

    [Fact]
    public void IfStatementTestMethod6()
    {
        var _false = () => false;
        var actual = _false.Fluent().If<int>().Then(() => 1).Else(() => 2);
        Assert.Equal(2, actual);
    }
}