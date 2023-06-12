using Library.DesignPatterns.Markers;
using Library.Validations;

using SysDriveInfo = System.IO.DriveInfo;
using SysFileInfo = System.IO.FileInfo;
using SysFolderInfo = System.IO.DirectoryInfo;

namespace Library.IO;

[Immutable]
public abstract record FileSystem
{
    /// <summary>
    /// Gets the full path.
    /// </summary>
    /// <value>
    /// The full path.
    /// </value>
    public abstract string FullPath { get; }

    /// <summary>
    /// Compares the FullPath of this FileSystem object to the FullPath of current instance.
    /// </summary>
    /// <param name="other">The other FileSystem object to compare to.</param>
    /// <returns>True if the FullPaths of both objects are equal, false otherwise.</returns>
    public virtual bool Equals(FileSystem? other)
        => other is { } o && o.FullPath.Equals(this.FullPath);

    /// <summary>
    /// Gets the hash code.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
        => this.FullPath.GetHashCode();

    /// <summary>
    /// Performs an implicit conversion from <see cref="FileSystem"/> to <see cref="string"/>.
    /// </summary>
    /// <param name="fileSystem">The file system.</param>
    /// <returns>
    /// The result of the conversion.
    /// </returns>
    public static implicit operator string([DisallowNull] in FileSystem fileSystem)
        => fileSystem.NotNull().FullPath;
}

[Immutable]
public sealed record Drive([DisallowNull] in SysDriveInfo driveInfo) : FileSystem
{
    private readonly SysDriveInfo _sysDriveInfo = driveInfo.ArgumentNotNull();

    /// <summary>
    /// Performs an explicit conversion from <see cref="SysDriveInfo?"/> to <see cref="Drive?"/>.
    /// </summary>
    /// <param name="driveInfo">The drive information.</param>
    /// <returns>
    /// The result of the conversion.
    /// </returns>
    public static explicit operator Drive?(in SysDriveInfo? driveInfo)
        => driveInfo is null ? null : new(driveInfo);
    /// <summary>
    /// Performs an explicit conversion from <see cref="Drive?"/> to <see cref="SysDriveInfo?"/>.
    /// </summary>
    /// <param name="drive">The drive.</param>
    /// <returns>
    /// The result of the conversion.
    /// </returns>
    public static explicit operator SysDriveInfo?(Drive? drive)
        => drive is null ? null : new(drive.FullPath[0].ToString());

    /// <summary>
    /// Gets the drives.
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<Drive> GetDrives()
        => SysDriveInfo.GetDrives().Select(FromSystemIoDriveInfo);

    /// <summary>
    /// Gets the root folder.
    /// </summary>
    /// <returns></returns>
    public Folder GetRootFolder()
        => new(this.FullPath);
    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    public string Name => this._sysDriveInfo.Name;

    public override string FullPath => this._sysDriveInfo.Name;

    [return: NotNull]
    public static Drive FromSystemIoDriveInfo([DisallowNull] SysDriveInfo driveInfo)
        => new(driveInfo);

    /// <summary>
    /// Converts to <see cref="SysDriveInfo"/>.
    /// </summary>
    /// <returns></returns>
    [return: NotNull]
    public SysDriveInfo ToSystemIoDriveInfo()
        => new(this.FullPath[0].ToString());
}

[Immutable]
public sealed record Folder([DisallowNull] SysFolderInfo directory) : FileSystem
{
    private readonly SysFolderInfo _sysFolder = directory.ArgumentNotNull();

    public override string FullPath => this._sysFolder.FullName;

    /// <summary>
    /// Initializes a new instance of the <see cref="Folder"/> class.
    /// </summary>
    /// <param name="fullPath">The full path.</param>
    public Folder([DisallowNull] string fullPath)
        : this(new SysFolderInfo(fullPath))
    {
    }

    /// <summary>
    /// Gets the folders.
    /// </summary>
    /// <param name="fullPath">The full path.</param>
    /// <returns></returns>
    [return: NotNull]
    public static IEnumerable<Folder> GetFolders(string fullPath)
        => new Folder(fullPath).GetFolders();

    /// <summary>
    /// Gets the folders.
    /// </summary>
    /// <returns></returns>
    [return: NotNull]
    public IEnumerable<Folder> GetFolders()
        => this._sysFolder.GetDirectories().Select(FromSystemIoDirectoryInfo);

    public static Folder FromSystemIoDirectoryInfo([NotNull] SysFolderInfo directory)
        => new(directory.NotNull());

    /// <summary>
    /// Gets the files.
    /// </summary>
    /// <param name="searchPattern">The search pattern.</param>
    /// <returns></returns>
    public IEnumerable<File> GetFiles(string searchPattern)
        => this._sysFolder.GetFiles(searchPattern).Select(x => new File(x));
    /// <summary>
    /// Gets the files.
    /// </summary>
    /// <param name="searchPattern">The search pattern.</param>
    /// <param name="enumerationOptions">The enumeration options.</param>
    /// <returns></returns>
    public IEnumerable<File> GetFiles(string searchPattern, EnumerationOptions enumerationOptions)
        => this._sysFolder.GetFiles(searchPattern, enumerationOptions).Select(x => new File(x));
    /// <summary>
    /// Gets the files.
    /// </summary>
    /// <param name="searchPattern">The search pattern.</param>
    /// <param name="searchOption">The search option.</param>
    /// <returns></returns>
    public IEnumerable<File> GetFiles(string searchPattern, SearchOption searchOption)
        => this._sysFolder.GetFiles(searchPattern, searchOption).Select(x => new File(x));
    /// <summary>
    /// Gets the files.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<File> GetFiles()
        => this._sysFolder.GetFiles().Select(x => new File(x));

    /// <summary>
    /// Performs an explicit conversion from <see cref="SysFolderInfo?"/> to <see cref="Folder?"/>.
    /// </summary>
    /// <param name="driveInfo">The drive information.</param>
    /// <returns>
    /// The result of the conversion.
    /// </returns>
    public static explicit operator Folder?(SysFolderInfo? driveInfo)
        => driveInfo is null ? null : new(driveInfo.FullName);
    /// <summary>
    /// Performs an explicit conversion from <see cref="Folder?"/> to <see cref="SysFolderInfo?"/>.
    /// </summary>
    /// <param name="folder">The folder.</param>
    /// <returns>
    /// The result of the conversion.
    /// </returns>
    public static explicit operator SysFolderInfo?(Folder? folder)
        => folder is null ? null : new(folder.FullPath);
}

[Immutable]
public sealed record File(SysFileInfo sysFileInfo) : FileSystem
{
    private readonly SysFileInfo _sysFileInfo = sysFileInfo;

    /// <summary>
    /// Initializes a new instance of the <see cref="File"/> class.
    /// </summary>
    /// <param name="fullPath">The full path.</param>
    public File(string fullPath)
        : this(new SysFileInfo(fullPath))
    {
    }

    public override string FullPath => this._sysFileInfo.FullName;
}