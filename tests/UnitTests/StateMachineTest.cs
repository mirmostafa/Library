using Library.DesignPatterns.StateMachine;

using Xunit.Abstractions;

namespace UnitTests;

[Trait("Category", nameof(Library.DesignPatterns))]
public sealed class StateMachineTest
{
    private readonly ITestOutputHelper _output;

    public StateMachineTest(ITestOutputHelper output)
        => this._output = output;

    [Fact]
    public async Task StateMachineFullTest()
    {
        _ = await StateMachineManager.Dispatch(
                        () => Task.FromResult((0, MoveDirection.Forward)),
                        flow => Task.FromResult(move(flow)),
                        flow => Task.FromResult(move(flow)),
                        display,
                        display);
        this._output.WriteLine("End.");

        (int Current, MoveDirection Direction) move((int Current, IEnumerable<(int State, MoveDirection Direction)>) flow)
        {
            this._output.WriteLine($"Current: {flow.Current}");
            MoveDirection direction;
            var randomNumber = new Random().Next(3);
            switch (randomNumber)
            {
                case 1:
                    flow.Current++;
                    direction = MoveDirection.Forward;
                    break;

                case 2:
                    flow.Current--;
                    direction = MoveDirection.Backward;
                    break;

                default:
                    direction = MoveDirection.Ended;
                    break;
            }
            return (flow.Current, direction);
        }

        Task display((int Current, IEnumerable<(int State, MoveDirection Direction)> History) flow)
        {
            this._output.WriteLine(flow.Current.ToString());
            flow.History.ForEach(x => this._output.WriteLine(x.ToString()));
            this._output.WriteLine("==================");
            return Task.CompletedTask;
        }
    }
}