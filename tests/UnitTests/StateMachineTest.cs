using Library.DesignPatterns.StateMachine;
using Library.Helpers.ConsoleHelper;


using Xunit.Abstractions;

namespace UnitTests;

public class StateMachineTest
{
    private readonly ITestOutputHelper _output;

    public StateMachineTest(ITestOutputHelper output) 
        => this._output = output;

    static public IEnumerable<object[]> Data => new[] { new object[] { ConsoleKey.UpArrow }, new object[] { ConsoleKey.DownArrow }, new object[] { ConsoleKey.End } };

    [Theory]
    [MemberData(nameof(Data))]
    public async Task StateMachineFullTest(ConsoleKey path)
    {
        _ = await StateMachineManager.Dispatch(
                        () => Task.FromResult((0, MoveDirection.Foreword)),
                        flow => Task.FromResult(move(flow)),
                        flow => Task.FromResult(move(flow)),
                        display,
                        display);
        this._output.WriteLine("End.");

        (int Current, MoveDirection Direction) move((int Current, IEnumerable<(int State, MoveDirection Direction)>) flow)
        {
            this._output.WriteLine($"Current: {flow.Current}. Press Up or Down to move foreword or backward. Or press any other key to done.");
            MoveDirection direction;
            switch (path)
            {
                case ConsoleKey.UpArrow:
                    flow.Current++;
                    direction = MoveDirection.Foreword;
                    break;

                case ConsoleKey.DownArrow:
                    flow.Current--;
                    direction = MoveDirection.Backword;
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
            _ = flow.History.ForEachEager(x => this._output.WriteLine(x.ToString()));
            this._output.WriteLine("==================");
            return Task.CompletedTask;
        }
    }
}