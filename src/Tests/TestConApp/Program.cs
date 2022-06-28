using Library.DesignPatterns.StateMachine;
using Library.Helpers;
using Library.IO;

namespace TestConApp;

internal partial class Program
{
    private static async Task Main()
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
            WriteLine($"Current: {flow.Current}. Press Up or Down to move foreword or backword. Or press any other key to done.");
            var resp = ReadKey().Key;
            MoveDirection direction;
            switch (resp)
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
                onChanged: e => WriteLine($"{e.Item.FullName} changed."),
                onCreated: e => WriteLine($"{e.Item.FullName} created."),
                onDeleted: e => WriteLine($"{e.Item.FullName} deleted."),
                onRenamed: e => WriteLine($"in {e.Item.FullPath} : {e.Item.OldName} renamed to {e.Item.NewName}."));
    }
}