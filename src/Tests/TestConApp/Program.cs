using Library.DesignPatterns;
using Library.Helpers;
using Library.IO;

namespace TestConApp;

internal partial class Program
{
    private static void Main()
    {
        var stateMachine = new NumericStateMachine(3);
        var last = stateMachine.MoveOverStateMachine(WriteLine);
        WriteLine($"Last: {last}");
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

internal class NumericStateMachine : IStateMachine<int>
{
    private readonly int _max;

    public NumericStateMachine(int max) => this._max = max;

    public FlowItem<int> MoveNext(int current)
        => new(current + 1, current + 1 > this._max - 1);

    public FlowItem<int> Start()
        => new(0, false);
}