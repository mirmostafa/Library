using Library.Threading.MultistepProgress;

namespace UnitTest;

[TestClass]
public class TaskRunnerTest
{
    [TestMethod]
    public async void SimpleTest1()
    {
        var add5 = (int x) => x + 5;
        var init = () => 5;

        var result = await TaskRunner<int>.New()
            .StartWith(init.ToAsync())
            .Then(add5.ToAsync())
            .Then(add5.ToAsync())
            .RunAsync();
        Assert.AreEqual(15, result);
    }
}