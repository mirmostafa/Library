using Library.Helpers;
using Library.Io;

namespace TestConApp;

internal partial class Program
{
    private static void Main()
    {
        Thread.Sleep(50);
        var watchers = Drive.GetAll().Select(getWatcher).ToList();
        watchers.ForEachEager(x => Console.WriteLine($"Warching {x.Path}"));
        Console.WriteLine("Ready");
        Console.ReadKey();
        Console.WriteLine("Closing...");
        watchers.ForEachEager(x => x.Dispose());

        static Library.Io.FileSystemWatcher getWatcher(Drive drive)
            => Library.Io.FileSystemWatcher.Start(drive, includeSubdirectories: true,
                onChanged: e => Console.WriteLine($"{e.Item.FullName} changed."),
                onCreated: e => Console.WriteLine($"{e.Item.FullName} created."),
                onDeleted: e => Console.WriteLine($"{e.Item.FullName} deleted."),
                onRenamed: e => Console.WriteLine($"in {e.Item.FullPath} : {e.Item.OldName} renamed to {e.Item.NewName}."));
    }
}