﻿#pragma warning disable IDE0051 // Remove unused private members
using System.Numerics;

using Library.CodeGeneration.v2.Front;
using Library.CodeGeneration.v2.Front.HtmlGeneration;
using Library.DesignPatterns;
using Library.DesignPatterns.StateMachine;
using Library.Helpers;
using Library.IO;

using Microsoft.Extensions.Logging;

namespace ConAppTest;

internal partial class Obsoletes
{
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

    private static IEnumerable<TResult> SimdActor<T1, T2, TResult>(
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

    private static async Task StateMachineTest()
    {
        _ = await StateMachineManager.Dispatch(
                        () => Task.FromResult((0, MoveDirection.Foreword)),
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

        static Task display((int Current, IEnumerable<(int State, MoveDirection Direction)> History) flow)
        {
            WriteLine(flow.Current);
            _ = flow.History.ForEachEager(x => Write(x));
            WriteLine();
            WriteLine("==================");
            return Task.CompletedTask;
        }
    }

    private static void WatchHardDisk()
    {
        Thread.Sleep(50);
        var watchers = Drive.GetDrives().Select(getWatcher).ForEach(x => WriteLine($"Watching {x.Path}")).ToList();
        WriteLine("Ready");
        _ = ReadKey();
        WriteLine("Closing...");
        _ = watchers.ForEachEager(x => x.Dispose());

        static Library.IO.FileSystemWatcher getWatcher(Drive drive)
            => Library.IO.FileSystemWatcher.Start(drive, includeSubdirectories: true,
                onChanged: e => _logger.Log($"{e.Item.FullName} changed."),
                onCreated: e => _logger.Log($"{e.Item.FullName} created."),
                onDeleted: e => _logger.Log($"{e.Item.FullName} deleted."),
                onRenamed: e => _logger.Log($"in {e.Item.FullPath} : {e.Item.OldName} renamed to {e.Item.NewName}."));
    }

    private static readonly Library.Logging.ILogger _logger = ConsoleServices.Logger;
}
#pragma warning restore IDE0051 // Remove unused private members