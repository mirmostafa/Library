using Library.DesignPatterns.Markers;

namespace Library.Io;

[Immutable]
public abstract class FileSystem
{
    public FileSystem(string fullPath!!)
        => this.FullPath = fullPath;

    public string FullPath { get; }
}
[Immutable]
public sealed class Drive : FileSystem
{
    public Drive(char driveLetter) : base($"""{driveLetter}:\""")
    {
    }
}
[Immutable]
public sealed class Folder : FileSystem
{
    public Folder(string fullPath) : base(fullPath)
    {
    }
}
[Immutable]
public sealed class File : FileSystem
{
    public File(string fullPath) : base(fullPath)
    {
    }
}
public static class FileSystemHelper
{
    public static string? Name(this FileSystem item)
        => Path.GetFileNameWithoutExtension(item.FullPath);
    public static string? Extension(this File item)
        => Path.GetExtension(item.FullPath);
    public static string? FolderName(this FileSystem item)
        => Path.GetDirectoryName(item.FullPath);
    public static string? Root(this Drive drive)
        => drive.FullPath;
}