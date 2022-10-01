
using Library.Coding;

namespace LibraryTest;
[TestClass]
public class FunctionalTest
{
    private static readonly Action<int> _emptyIntAction = x => { };
    private static readonly Func<int> _returnsTwo = () => 2;
    private record EmptyRecord();


    [TestMethod]
    public void IfConditionTest()
    {
        var bolleanTest = true;
        var trueResult = IfTrue(bolleanTest, () => "true");
        //IfTrue(bolleanTest, Methods.Empty);
        Assert.AreEqual("true", trueResult);
    }

    [TestMethod]
    public void IfConditionTest2()
    {
        var bolleanTest = false;
        var falseResult = bolleanTest.IfFalse(() => "false");
        _ = bolleanTest.IfFalse(Methods.Empty);

        Assert.AreEqual("false", falseResult);
    }

    [TestMethod]
    public void FluentTest()
    {
        this.Fluent(Methods.Empty);
        var two = 2.Fluent<int>(() => { });
        Assert.AreEqual(2, two.Value);
    }

    [TestMethod]
    public void FluentByResultTest()
    {
        var actualTwo = 2.Fluent().Result(_returnsTwo);
        Assert.AreEqual((2, 2), actualTwo);
    }

    [TestMethod]
    public void FluentByResultTest2()
    {

        var actualThree = 3.Fluent().Result(_returnsTwo);
        Assert.AreEqual((3, 2), actualThree);
    }

    //[TestMethod]
    //public async void FluentByResultAsyncTest()
    //{
    //    var actualTask = await 2.FluentAsync(Methods.SelfTask<int>());
    //    Assert.AreEqual(2, actualTask);
    //}

    [TestMethod]
    public void ForEachTest() => Enumerable.Range(1, 1).ForEach(_emptyIntAction);

    [TestMethod]
    public void LockTest() => Lock(this, Methods.Empty);

    [TestMethod]
    public void NewTest() => New<EmptyRecord>();

    [TestMethod]
    public void WhileTest() => While(() => false);

    [TestMethod]
    public void WhileFuncTest() => _ = While(() => false, _returnsTwo);
}
