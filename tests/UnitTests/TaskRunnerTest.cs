using Library.Threading.MultistepProgress;

namespace UnitTests;


[Trait("Category", "Code Helpers")]
public sealed class TaskRunnerTest
{
    [Theory]
    [InlineData(5, 5, 15)]
    [InlineData(1, 1, 3)]
    [InlineData(10, 1, 12)]
    [InlineData(-5, 5, 5)]
    public async void SimpleTest1(int initialValue, int steps, int expected)
    {
        var add5 = (int x) => x + steps;
        var init = () => initialValue;
        var actual = await TaskRunner<int>.New()
            .StartWith(init)
            .Then(add5)
            .Then(add5)
            .RunAsync();
        Assert.Equal(expected, actual);
    }
}