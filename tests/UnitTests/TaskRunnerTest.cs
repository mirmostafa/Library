using Library.Threading;

namespace UnitTests;

[Trait("Category", nameof(Library.Coding))]
[Trait("Category", nameof(TaskRunner))]
public sealed class TaskRunnerTest
{
    [Theory]
    [InlineData(5, 5, 15)]
    [InlineData(1, 1, 3)]
    [InlineData(10, 1, 12)]
    [InlineData(-5, 5, 5)]
    public async void SimpleTest1(int initialValue, int steps, int expected)
    {
        var actual = await TaskRunner<int>.StartWith(init)
            .Then(add5)
            .Then(add5)
            .RunAsync();
        Assert.Equal(expected, actual);

        int add5(int x) => x + steps;
        int init() => initialValue;
    }
}