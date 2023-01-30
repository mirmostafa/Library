using Library.Threading.MultistepProgress;

namespace UnitTests;


public class TaskRunnerTest
{
    [Fact]
    public void SimpleTest1()
    {
        var add5 = (int x) => x + 5;
        var init = () => 5;

        var result = TaskRunner<int>.New()
            .StartWith(init.ToAsync())
            .Then(add5.ToAsync())
            .Then(add5.ToAsync())
            .RunAsync().Result;
        Assert.Equal(15, result);
    }
}