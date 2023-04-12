using Library.Coding;

namespace UnitTests;


[Trait("Category", "Code Helpers")]
[Obsolete("Subject to remove", true)]
public sealed class IfStatementTest
{
    [Fact(Skip = $"Subject to remote taste case: {nameof(Args<int>)}")]
    public void IfStatementTestMethod1()
        => true.Fluent().If().Then(Methods.Empty).Else(Methods.Empty);

    [Fact(Skip = $"Subject to remote taste case: {nameof(Args<int>)}")]
    public void IfStatementTestMethod2()
    {
        var actual = true.Fluent().If<int>().Then(() => 1).Else(() => 2);
        Assert.Equal(1, actual);
    }

    [Fact(Skip = $"Subject to remote taste case: {nameof(Args<int>)}")]
    public void IfStatementTestMethod3()
    {
        var actual = false.Fluent().If<int>().Then(() => 1).Else(() => 2);
        Assert.Equal(2, actual);
    }

    [Fact(Skip = $"Subject to remote taste case: {nameof(Args<int>)}")]
    public void IfStatementTestMethod4()
    {
        var _true = () => true;
        _true.Fluent().If().Then(Methods.Empty).Else(Methods.Empty);
    }

    [Fact(Skip = $"Subject to remote taste case: {nameof(Args<int>)}")]
    public void IfStatementTestMethod5()
    {
        var _true = () => true;
        var actual = _true.Fluent().If<int>().Then(() => 1).Else(() => 2);
        Assert.Equal(1, actual);
    }

    [Fact(Skip = $"Subject to remote taste case: {nameof(Args<int>)}")]
    public void IfStatementTestMethod6()
    {
        var _false = () => false;
        var actual = _false.Fluent().If<int>().Then(() => 1).Else(() => 2);
        Assert.Equal(2, actual);
    }
}