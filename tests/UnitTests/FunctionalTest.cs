using Library.Coding;

namespace UnitTests;

[Trait("Category", "Code Helpers")]
public class FunctionalTest
{
    private static readonly Action<int> _emptyIntAction = x => { };
    private static readonly Func<int> _returnsTwo = () => 2;
    private record EmptyRecord();


    [Fact]
    public void IfConditionTest()
    {
        var booleanTest = true;
        var trueResult = booleanTest.IfTrue(() => "true");
        Assert.Equal("true", trueResult);
    }

    [Fact]
    public void IfConditionTest2()
    {
        var booleanTest = false;
        var falseResult = booleanTest.IfFalse(() => "false");
        _ = booleanTest.IfFalse(Methods.Empty);

        Assert.Equal("false", falseResult);
    }

    [Fact]
    public void FluentTest()
    {
        this.Fluent(Methods.Empty);
        var two = 2.Fluent(() => { });
        Assert.Equal(2, two.Value);
    }

    [Fact]
    public void FluentByResultTest()
    {
        var actualTwo = 2.Fluent().Result(_returnsTwo);
        Assert.Equal((2, 2), actualTwo);
    }

    [Fact]
    public void FluentByResultTest2()
    {

        var actualThree = 3.Fluent().Result(_returnsTwo);
        Assert.Equal((3, 2), actualThree);
    }

    //[Fact]
    //public async void FluentByResultAsyncTest()
    //{
    //    var actualTask = await 2.FluentAsync(Methods.SelfTask<int>());
    //    Assert.Equal(2, actualTask);
    //}

    [Fact]
    public void ForEachTest() => Enumerable.Range(1, 1).ForEach(_emptyIntAction);

    [Fact]
    public void LockTest() => Lock(this, Methods.Empty);

    [Fact]
    public void NewTest() => New<EmptyRecord>();

    [Fact]
    public void WhileTest() => While(() => false);

    [Fact]
    public void WhileFuncTest() => _ = While(() => false, _returnsTwo);
}
