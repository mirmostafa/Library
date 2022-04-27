using Library.Io;

namespace TestConApp;

internal partial class Program
{
    private static void Main(params string[] args)
    {
        Console.WriteLine("Initializing....");
        Thread.Sleep(1000);
        using (Library.Io.FileSystemWatcher.Start(@"C:\", includeSubdirectories: true,
            onChanged: e => Console.WriteLine($"{e.Item.FullName} changed."),
            onCreated: e => Console.WriteLine($"{e.Item.FullName} created."),
            onDeleted: e => Console.WriteLine($"{e.Item.FullName} deleted."),
            onRenamed: e => Console.WriteLine($"in {e.Item.FullPath} : {e.Item.OldName} renamed to {e.Item.NewName}.")
            ))
        {
            Console.WriteLine("Initiated.");
            _ = Console.ReadKey();
        }
    }
}