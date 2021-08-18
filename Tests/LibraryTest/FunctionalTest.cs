
using Library.Coding;
using Library.Helpers;

namespace LibraryTest;
[TestClass]
public class FunctionalTest
{
    private static readonly Action EmptyAction = () => { };
    private static readonly Action<int> EmptyIntAction = x => { };
    private static readonly Func<int, int> SelfIntFunc = x => x;
    private static readonly Func<Task> ReturnsTask = async () => await Task.CompletedTask;
    private static readonly Func<int> ReturnsTwo = () => 2;
    private static readonly Func<Task<int>> ReturnsTaskTwo = async () => await Task.FromResult(2);
    private record EmptyRecord();


    [TestMethod]
    public void IfConditionTest()
    {
        var bolleanTest = true;
        var trueResult = bolleanTest.IfTrue(() => "true");
        _ = bolleanTest.IfTrue(EmptyAction);

        bolleanTest = false;
        var falseResult = bolleanTest.IfFalse(() => "false");
        _ = bolleanTest.IfFalse(EmptyAction);

        Assert.AreEqual("true", trueResult);
        Assert.AreEqual("false", falseResult);
    }

    [TestMethod]
    public void FluentTest()
    {
        this.Fluent(EmptyAction);
        var two = Fluent<int>(2, _ => { });
        Assert.AreEqual(2, two);
    }

    [TestMethod]
    public void FluentByResultTest()
    {
        var actualTwo = 2.FluentByResult(ReturnsTwo);
        Assert.AreEqual((2, 2), actualTwo);

        var actualThree = 3.FluentByResult(ReturnsTwo);
        Assert.AreEqual((3, 2), actualThree);
    }

    [TestMethod]
    public async void FluentByResultAsyncTest()
    {
        var actualTask = await 2.FluentAsync(ReturnsTask);
        Assert.AreEqual(2, actualTask);
    }

    [TestMethod]
    public void ForEachTest() => Enumerable.Range(1, 1).ForEach(EmptyIntAction);

    [TestMethod]
    public void LockTest() => this.Lock(EmptyAction);

    [TestMethod]
    public void NewTest() => New<EmptyRecord>();

    [TestMethod]
    public void WhileTest() => While(() => false);

    [TestMethod]
    public void WhileFuncTest() => _ = While(() => false, ReturnsTwo, null);

}
