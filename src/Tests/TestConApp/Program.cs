using Library.Helpers;
using Library.IO;

namespace TestConApp;

internal partial class Program
{
    private static void Main()
    {
        Thread.Sleep(50);
        var watchers = Drive.GetDrives().Select(getWatcher).ToList();
        watchers.ForEachEager(x => Console.WriteLine($"Watching {x.Path}"));
        Console.WriteLine("Ready");
        Console.ReadKey();
        Console.WriteLine("Closing...");
        watchers.ForEachEager(x => x.Dispose());

        static Library.IO.FileSystemWatcher getWatcher(Drive drive)
            => Library.IO.FileSystemWatcher.Start(drive, includeSubdirectories: true,
                onChanged: e => Console.WriteLine($"{e.Item.FullName} changed."),
                onCreated: e => Console.WriteLine($"{e.Item.FullName} created."),
                onDeleted: e => Console.WriteLine($"{e.Item.FullName} deleted."),
                onRenamed: e => Console.WriteLine($"in {e.Item.FullPath} : {e.Item.OldName} renamed to {e.Item.NewName}."));
    }
}