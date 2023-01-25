using Library.Threading.MultistepProgress;

namespace Library.UnitTest;

[TestClass]
public class TaskRunnerTest
{
    [TestMethod]
    public void SimpleTest1()
    {
        var add5 = (int x) => x + 5;
        var init = () => 5;

        var result = TaskRunner<int>.New()
            .StartWith(init.ToAsync())
            .Then(add5.ToAsync())
            .Then(add5.ToAsync())
            .RunAsync().Result;
        Assert.AreEqual(15, result);
    }
}