using System.ComponentModel;

using Library.EventsArgs;
using Library.Interfaces;
using Library.Validations;

using EventArgsItem = (string FullName, string? Name);
using RenamedItem = (string FullPath, string OldName, string NewName);

namespace Library.IO;

public sealed class FileSystemWatcher : IDisposable, ISupportSilence
{
    private readonly CancellationToken _cancellationToken;
    private readonly string _path;
    private readonly System.IO.FileSystemWatcher _innerWatcher;
    private readonly Action _waitForChanged;
    private readonly Task _watcherTask;
    private bool _disposedValue;

    /// <summary>
    /// Occurs when [changed].
    /// </summary>
    public event EventHandler<ChangedEventArgs>? Changed;

    /// <summary>
    /// Occurs when a file system is [created].
    /// </summary>
    public event EventHandler<CreatedEventArgs>? Created;

    /// <summary>
    /// Occurs when a file system is [deleted].
    /// </summary>
    public event EventHandler<DeletedEventArgs>? Deleted;

    /// <summary>
    /// Occurs when an error occurred.
    /// </summary>
    public event EventHandler<ErrorEventArgs>? Error;

    /// <summary>
    /// Occurs when a file system is [renamed].
    /// </summary>
    public event EventHandler<RenamedEventArgs>? Renamed;

    /// <summary>
    /// Constructs a new instance of the <see cref="FileSystemWatcher"/> class with the specified
    /// path, wildcard and includeSubdirectories.
    /// </summary>
    /// <param name="path">The path of the directory to watch.</param>
    /// <param name="wildcard">The wildcard filter to use.</param>
    /// <param name="includeSubdirectories">A value indicating whether to include subdirectories.</param>
    /// <returns>A new instance of the <see cref="FileSystemWatcher"/> class.</returns>
    public FileSystemWatcher(in string path, in string? wildcard = null, in bool includeSubdirectories = false, in CancellationToken cancellationToken = default)
    {
        this._path = path.ArgumentNotNull();
        this._innerWatcher = wildcard switch
        {
            null => new System.IO.FileSystemWatcher(this._path) { IncludeSubdirectories = includeSubdirectories },
            not null => new System.IO.FileSystemWatcher(this._path, wildcard) { IncludeSubdirectories = includeSubdirectories },
        };
        this._innerWatcher.BeginInit();
        this._innerWatcher.NotifyFilter =
                           System.IO.NotifyFilters.Attributes |
                           System.IO.NotifyFilters.CreationTime |
                           System.IO.NotifyFilters.DirectoryName |
                           System.IO.NotifyFilters.FileName |
                           System.IO.NotifyFilters.LastAccess |
                           System.IO.NotifyFilters.LastWrite |
                           System.IO.NotifyFilters.Security |
                           System.IO.NotifyFilters.Size;
        this._innerWatcher.Changed += this._innerWatcher_Changed;
        this._innerWatcher.Created += this._innerWatcher_Created;
        this._innerWatcher.Deleted += this._innerWatcher_Deleted;
        this._innerWatcher.Disposed += this._innerWatcher_Disposed;
        this._innerWatcher.Error += this._innerWatcher_Error;
        this._innerWatcher.Renamed += this._innerWatcher_Renamed;
        this._innerWatcher.EndInit();
        this._waitForChanged = () =>
        {
            while (!this._disposedValue && !this._cancellationToken.IsCancellationRequested)
            {
                var a = this._innerWatcher.WaitForChanged(WatcherChangeTypes.All);
            }
        };
        this._cancellationToken = cancellationToken;
        this._watcherTask = new Task(this._waitForChanged, this._cancellationToken);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the component is enabled to raise events.
    /// </summary>
    public bool IsEnabledRaisingEvents
    {
        get => this._innerWatcher.EnableRaisingEvents;
        set => this._innerWatcher.EnableRaisingEvents = value;
    }

    public string Path => this._innerWatcher.Path;

    public static FileSystemWatcher New(in string path, in string? wildcard = null, in bool includeSubdirectories = false, in CancellationToken cancellationToken = default)
        => new(path, wildcard, includeSubdirectories, cancellationToken);

    /// <summary>
    /// Starts a FileSystemWatcher with the given parameters and returns the FileSystemWatcher object.
    /// </summary>
    /// <param name="path">The path to watch.</param>
    /// <param name="wildcard">The wildcard to use for filtering.</param>
    /// <param name="includeSubdirectories">Whether to include subdirectories.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="onCreated">The action to take when a file is created.</param>
    /// <param name="onRenamed">The action to take when a file is renamed.</param>
    /// <param name="onChanged">The action to take when a file is changed.</param>
    /// <param name="onDeleted">The action to take when a file is deleted.</param>
    /// <param name="onError">The action to take when an error occurs.</param>
    /// <returns>The FileSystemWatcher object.</returns>
    public static FileSystemWatcher Start(
        in string path, in string? wildcard = null, in bool includeSubdirectories = false,
        Action<CreatedEventArgs>? onCreated = null,
        Action<RenamedEventArgs>? onRenamed = null,
        Action<ChangedEventArgs>? onChanged = null,
        Action<DeletedEventArgs>? onDeleted = null,
        Action<ErrorEventArgs>? onError = null,
        CancellationToken cancellationToken = default
        )
    {
        var result = New(path, wildcard, includeSubdirectories, cancellationToken);

        if (onChanged is not null)
        {
            result.Changed += (s, e) => onChanged(e);
        }
        if (onCreated is not null)
        {
            result.Created += (s, e) => onCreated(e);
        }
        if (onRenamed is not null)
        {
            result.Renamed += (s, e) => onRenamed(e);
        }
        if (onDeleted is not null)
        {
            result.Deleted += (s, e) => onDeleted(e);
        }
        if (onError is not null)
        {
            result.Error += (s, e) => onError(e);
        }
        return result.Start();
    }

    /// <summary>
    /// Disposes the object and releases any associated resources.
    /// </summary>
    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public FileSystemWatcher OnChanged(EventHandler<ChangedEventArgs> handler)
    {
        this.Changed -= handler;
        this.Changed += handler;
        return this;
    }

    public FileSystemWatcher OnCreated(EventHandler<CreatedEventArgs> handler)
    {
        this.Created -= handler;
        this.Created += handler;
        return this;
    }

    public FileSystemWatcher OnDeleted(EventHandler<DeletedEventArgs> handler)
    {
        this.Deleted -= handler;
        this.Deleted += handler;
        return this;
    }

    public FileSystemWatcher OnRenamed(EventHandler<RenamedEventArgs> handler)
    {
        this.Renamed -= handler;
        this.Renamed += handler;
        return this;
    }

    /// <summary>
    /// Starts the FileSystemWatcher and returns it.
    /// </summary>
    /// <returns>The FileSystemWatcher.</returns>
    public FileSystemWatcher Start() => this.Fluent<FileSystemWatcher>(() =>
    {
        _ = this.ThrowIfDisposed(this._disposedValue);
        if (this._watcherTask.Status is not TaskStatus.Running or TaskStatus.WaitingToRun)
        {
            this._watcherTask.Start();
        }
    });

    private void _innerWatcher_Changed(object sender, FileSystemEventArgs e)
        => this.Changed?.Invoke(this, new(new(e.FullPath, e.Name)));

    private void _innerWatcher_Created(object sender, FileSystemEventArgs e)
        => this.Created?.Invoke(this, new(new(e.FullPath, e.Name)));

    private void _innerWatcher_Deleted(object sender, FileSystemEventArgs e)
        => this.Deleted?.Invoke(this, new(new(e.FullPath, e.Name)));

    private void _innerWatcher_Disposed(object? sender, EventArgs e)
    {
        this._disposedValue = true;
        this.Dispose();
    }

    private void _innerWatcher_Error(object sender, System.IO.ErrorEventArgs e)
        => this.Error?.Invoke(this, new(e.GetException()));

    private void _innerWatcher_Renamed(object sender, System.IO.RenamedEventArgs e)
        => this.Renamed?.Invoke(this, new RenamedEventArgs(new(e.FullPath, e.OldName ?? e.OldFullPath, e.Name ?? e.FullPath)));

    private void Dispose(bool disposing)
    {
        if (!this._disposedValue)
        {
            if (disposing && this._innerWatcher is not null)
            {
                if (this._watcherTask.Status == TaskStatus.Running)
                {
                    //this._watcherTask.
                }
                this._innerWatcher.Dispose();
            }
            this._disposedValue = true;
        }
    }

    public override string ToString()
        => $"{this._watcherTask.Status}: {this._path}";

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public sealed class ChangedEventArgs(in EventArgsItem item) : FileSystemWatchEventArgs<EventArgsItem>(item)
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public sealed class CreatedEventArgs(in EventArgsItem item) : FileSystemWatchEventArgs<EventArgsItem>(item)
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public sealed class DeletedEventArgs(in EventArgsItem item) : FileSystemWatchEventArgs<EventArgsItem>(item)
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public sealed class ErrorEventArgs(in Exception item) : FileSystemWatchEventArgs<Exception>(item)
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public abstract class FileSystemWatchEventArgs<TItem>(in TItem item) : ItemActedEventArgs<TItem>(item)
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public sealed class RenamedEventArgs(in RenamedItem item) : FileSystemWatchEventArgs<RenamedItem>(item)
    {
    }
}

public enum NotifyFilters
{
    /// <devdoc>[To be supplied.]</devdoc>
    FileName = 0x00000001,

    /// <devdoc>[To be supplied.]</devdoc>
    DirectoryName = 0x00000002,

    /// <devdoc>The attributes of the file or folder.</devdoc>
    Attributes = 0x00000004,

    /// <devdoc>The size of the file or folder.</devdoc>
    Size = 0x00000008,

    /// <devdoc>The date that the file or folder last had anything written to it.</devdoc>
    LastWrite = 0x00000010,

    /// <devdoc>The date that the file or folder was last opened.</devdoc>
    LastAccess = 0x00000020,

    /// <devdoc>[To be supplied.]</devdoc>
    CreationTime = 0x00000040,

    /// <devdoc>The security settings of the file or folder.</devdoc>
    Security = 0x00000100,
}