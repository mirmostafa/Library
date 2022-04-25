using Library.DesignPatterns.Markers;
using Library.Validations;
using SysDriveInfo = System.IO.DriveInfo;

namespace Library.Io;

[Immutable]
public abstract class FileSystem : IEquatable<FileSystem>
{
    public FileSystem(string fullPath!!)
        => this.FullPath = fullPath;

    public string FullPath { get; }

    public bool Equals(FileSystem? other)
        => other is not null && other.FullPath.Equals(this.FullPath);

    public override bool Equals(object? obj)
        => obj is FileSystem fs && fs.Equals(this);

    public override int GetHashCode()
        => this.FullPath.GetHashCode();

    public static implicit operator string([DisallowNull] FileSystem fileSystem)
        => fileSystem.ArgumentNotNull().FullPath;
}

[Immutable]
public sealed class Drive : FileSystem
{
    public Drive(char driveLetter)
        : base($"""{driveLetter}:\""")
    {
    }

    public static explicit operator Drive?(SysDriveInfo? sysDrive)
        => sysDrive is null ? null : new(sysDrive.RootDirectory.FullName[0]);
    public static explicit operator SysDriveInfo?(Drive? drive)
        => drive is null ? null : new(drive.FullPath[0].ToString());

    [return: NotNull]
    public static IEnumerable<Drive> GetAll()
        => SysDriveInfo.GetDrives().Select(FromSystemDrive);

    [return: NotNull]
    public static Drive FromSystemDrive([DisallowNull] SysDriveInfo sysDrive)
        => new(sysDrive.ArgumentNotNull().RootDirectory.FullName[0]);

    [return: NotNull]
    public SysDriveInfo ToSystemDrive()
        => new(this.FullPath[0].ToString());
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