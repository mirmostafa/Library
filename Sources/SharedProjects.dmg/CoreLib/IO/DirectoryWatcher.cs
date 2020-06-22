using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Mohammad.EventsArgs;
using Mohammad.Helpers;
using Mohammad.Threading.Tasks;
using Mohammad.Win32.Natives;

namespace Mohammad.IO
{
    public class DirectoryWatcher
    {
        private readonly TaskScheduler _TaskScheduler;
        private CancellationTokenSource _CancellationTokenSource;
        private bool _EnableRaisingEvents;
        public bool WatchSubtree { get; set; } = true;
        public DirectoryInfo Path { get; }

        public DirectoryWatcher(string path)
            : this(new DirectoryInfo(path)) {}

        public DirectoryWatcher(DirectoryInfo path)
        {
            this.Path = path;
            //this._TaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            this._TaskScheduler = TaskScheduler.Current;
        }

        public event EventHandler<ItemActedEventArgs<string>> Added;
        protected virtual void OnAdded(ItemActedEventArgs<string> e) { this.Added.RaiseAsync(this, e, this._TaskScheduler); }
        protected virtual void OnDeleted(ItemActedEventArgs<string> e) { this.Deleted.RaiseAsync(this, e, this._TaskScheduler); }
        protected virtual void OnModified(ItemActedEventArgs<string> e) { this.Modified.RaiseAsync(this, e, this._TaskScheduler); }
        protected virtual void OnRenamed(ChangedEventArgs<string> e) { this.Renamed.RaiseAsync(this, e, this._TaskScheduler); }
        public event EventHandler<ItemActedEventArgs<string>> Deleted;
        public event EventHandler<ItemActedEventArgs<string>> Modified;
        public event EventHandler<ChangedEventArgs<string>> Renamed;

        private void Watch()
        {
            const uint bufsize = 2048;

            var hDir = Api.CreateFile(this.Path.FullName,
                WindowsConstants.FILE_LIST_DIRECTORY,
                WindowsConstants.FILE_SHARE_READ | WindowsConstants.FILE_SHARE_WRITE | WindowsConstants.FILE_SHARE_DELETE,
                IntPtr.Zero,
                WindowsConstants.OPEN_EXISTING,
                WindowsConstants.FILE_FLAG_BACKUP_SEMANTICS,
                IntPtr.Zero);
            if (hDir == IntPtr.Zero)
                throw new InvalidOperationException("Cannot initailize.");

            var pBuf = IntPtr.Zero;
            try
            {
                pBuf = Marshal.AllocHGlobal((int) bufsize);
                uint bytesReturned;
                while (this._EnableRaisingEvents &&
                       Api.ReadDirectoryChangesW(hDir,
                           pBuf,
                           bufsize,
                           this.WatchSubtree,
                           WindowsConstants.FILE_NOTIFY_CHANGE_FILE_NAME | WindowsConstants.FILE_NOTIFY_CHANGE_DIR_NAME |
                           WindowsConstants.FILE_NOTIFY_CHANGE_LAST_WRITE,
                           out bytesReturned,
                           IntPtr.Zero,
                           IntPtr.Zero))
                {
                    if (!this._EnableRaisingEvents)
                        break;
                    var pCurrent = pBuf;
                    string file = null;
                    string newNameFile = null;
                    var action = 0;
                    while (pCurrent != IntPtr.Zero)
                    {
                        var fileLen = Marshal.ReadInt32(pCurrent, 8);
                        newNameFile = Marshal.PtrToStringUni((IntPtr) (12 + (int) pCurrent), fileLen / 2);
                        action = Marshal.ReadInt32(pCurrent, 4);
                        if (action < 1 || action >= 6)
                            action = 0;
                        if (action != 5)
                            file = newNameFile;
                        var inc = Marshal.ReadInt32(pCurrent);
                        pCurrent = inc != 0 ? (IntPtr) (inc + (int) pCurrent) : IntPtr.Zero;
                    }
                    switch (action)
                    {
                        case 0:
                            break;
                        case 1:
                            this.OnAdded(new ItemActedEventArgs<string>(System.IO.Path.Combine(this.Path.FullName, file)));
                            break;
                        case 2:
                            this.OnDeleted(new ItemActedEventArgs<string>(System.IO.Path.Combine(this.Path.FullName, file)));
                            break;
                        case 3:
                        case 4:
                            this.OnModified(new ItemActedEventArgs<string>(System.IO.Path.Combine(this.Path.FullName, file)));
                            break;
                        case 5:
                            this.OnRenamed(new ChangedEventArgs<string>(System.IO.Path.Combine(this.Path.FullName, file), System.IO.Path.GetFileName(newNameFile)));
                            break;
                    }
                }
                if (this._EnableRaisingEvents)
                    throw new Exception("Reading Directory Changes failed. " + Marshal.GetLastWin32Error());
            }
            finally
            {
                if (pBuf != IntPtr.Zero)
                    Marshal.FreeHGlobal(pBuf);
                Api.CloseHandle(hDir);
            }
        }

        public async void Start()
        {
            if (this.Path == null || !this.Path.Exists)
                throw new DirectoryNotFoundException("Path is null or not found.");
            this._EnableRaisingEvents = true;
            this._CancellationTokenSource = new CancellationTokenSource();
            await Async.Run(this.Watch, this._CancellationTokenSource.Token);
        }

        public void Stop()
        {
            this._EnableRaisingEvents = false;
            this._CancellationTokenSource.Cancel();
            this._CancellationTokenSource.Dispose();
        }
    }
}