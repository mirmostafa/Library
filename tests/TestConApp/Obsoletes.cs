using System.Diagnostics.CodeAnalysis;
using System.Numerics;

using Library.CodeGeneration.v2.Front;
using Library.CodeGeneration.v2.Front.HtmlGeneration;
using Library.DesignPatterns.StateMachine;
using Library.IO;
using Library.Logging;

namespace TestConApp;

internal partial class Obsoletes
{
    private static readonly ILogger _logger = ConsoleServices.Logger;

    public static void ConstantExpectedTest([ConstantExpected(Max = 10)] int a) => Console.WriteLine(a);

    public static IEnumerable<long> Fibonacci(long count)
    {
        return FibonacciRecursiveIterator(count, 0L, 1L);

        static IEnumerable<long> FibonacciRecursiveIterator(long count, long a, long b)
        {
            if (count <= 0)
            {
                yield break;
            }

            yield return a;

            foreach (var number in FibonacciRecursiveIterator(count - 1, b, a + b))
            {
                yield return number;
            }
        }
    }

    public static void HtmlCodeGenerationTest()
    {
        var div = new DivElement()
                    .AddAttribute("row")
                    .AddChild(new InputElement("text"))
                    .AddChild(new DivElement().AddAttribute("class", "col")
                            .AddChild(new DivElement().AddAttribute("class", "col")
                                .AddChild(new InputElement("checkbox").AddAttribute("class", "text-box")))
                            .AddChild(new InputElement("password")))
                    .AddChild(new DivElement().AddAttribute("class", "col")
                            .AddChild(new DivElement().AddAttribute("class", "col")
                                .AddChild(new InputElement("checkbox").AddAttribute("class", "text-box")))
                            .AddChild(new InputElement("password")))
                    .AddChild(new DivElement().AddAttribute("class", "col")
                            .AddChild(new DivElement().AddAttribute("class", "col")
                                .AddChild(new InputElement("checkbox").AddAttribute("class", "text-box")))
                            .AddChild(new InputElement("password")))
                    .AddChild(new InputElement("text"));
        WriteLine(div.ToHtml());
    }

    public static IEnumerable<TResult> SimdActor<T1, T2, TResult>(
            IEnumerable<T1> left, IEnumerable<T2> right, Func<Vector<T1>, Vector<T2>, Vector<TResult>> actor, long count)
        where T1 : struct
        where T2 : struct
        where TResult : struct
    {
        var leftArray = left.ToArray();
        var rightArray = right.ToArray();
        var result = new TResult[count];
        var offset = Vector<TResult>.Count;
        for (var i = 0; i < count - offset; i += offset)
        {
            var v1 = new Vector<T1>(leftArray, i);
            var v2 = new Vector<T2>(rightArray, i);
            actor(v1, v2).CopyTo(result, i);
        }
        return result;
    }

    public static async Task StateMachineTest()
    {
        _ = await StateMachineManager.Dispatch(
                        () => Task.FromResult((0, MoveDirection.Forward)),
                        flow => Task.FromResult(move(flow)),
                        flow => Task.FromResult(move(flow)),
                        display,
                        display);
        WriteLine("End.");

        static (int Current, MoveDirection Direction) move((int Current, IEnumerable<(int State, MoveDirection Direction)>) flow)
        {
            WriteLine($"Current: {flow.Current}. Press Up or Down to move foreword or backward. Or press any other key to done.");
            var response = ReadKey().Key;
            MoveDirection direction;
            switch (response)
            {
                case ConsoleKey.UpArrow:
                    flow.Current++;
                    direction = MoveDirection.Forward;
                    break;

                case ConsoleKey.DownArrow:
                    flow.Current--;
                    direction = MoveDirection.Backward;
                    break;

                default:
                    direction = MoveDirection.Ended;
                    break;
            }
            return (flow.Current, direction);
        }

        static Task display((int Current, IEnumerable<(int State, MoveDirection Direction)> History) flow)
        {
            WriteLine(flow.Current);
            flow.History.ForEach(x => Write(x));
            WriteLine();
            WriteLine("==================");
            return Task.CompletedTask;
        }
    }

    public static void WatchHardDisk()
    {
        var watchers = Drive.GetDrives().Select(watch).CreateIterator(x => WriteLine($"Watching {x.Path}")).ToList();
        WriteLine("Ready");

        While(() => ReadKey().Key != ConsoleKey.X);
        WriteLine("Closing...");
        watchers.ForEach(x => x.Dispose());

        static Library.IO.FileSystemWatcher watch(Drive drive)
            => Library.IO.FileSystemWatcher.New(drive, includeSubdirectories: true)
                .OnCreated((_, e) => _logger.Log($"{e.Item.FullName} created.", format: LogFormat.FORMAT_SHORT))
                .OnChanged((_, e) => _logger.Log($"{e.Item.FullName} changed.", format: LogFormat.FORMAT_SHORT))
                .OnRenamed((_, e) => _logger.Log($"in {e.Item.FullPath} : {e.Item.OldName} renamed to {e.Item.NewName}.", format: LogFormat.FORMAT_SHORT))
                .OnDeleted((_, e) => _logger.Log($"{e.Item.FullName} deleted.", format: LogFormat.FORMAT_SHORT))
                .Start();
    }
}