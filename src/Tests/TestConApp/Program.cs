using Library.Helpers;
using Library.IO;

internal partial class Program
{
    private static void Main()
    {
        WatchHardDisk();
        _ = ReadLine();
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
                onChanged: e => Logger.Log($"{e.Item.FullName} changed."),
                onCreated: e => Logger.Log($"{e.Item.FullName} created."),
                onDeleted: e => Logger.Log($"{e.Item.FullName} deleted."),
                onRenamed: e => Logger.Log($"in {e.Item.FullPath} : {e.Item.OldName} renamed to {e.Item.NewName}."));
    }
}