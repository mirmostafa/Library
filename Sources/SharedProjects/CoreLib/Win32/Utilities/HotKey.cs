using System;
using System.Threading;
using System.Threading.Tasks;
using Mohammad.EventsArgs;
using Mohammad.Helpers;
using Mohammad.Win32.Natives;
using Mohammad.Win32.Natives.IfacesEnumsStructsClasses;

namespace Mohammad.Win32.Utilities
{
    public class HotKey : IDisposable
    {
        private Task _Task;
        protected KeyModifier Modifiers { get; }
        protected Keys Key { get; }
        public int Id { get; }
        protected TaskScheduler MainScheduler { get; private set; }
        protected CancellationTokenSource CancellationTokenSource { get; private set; } = new CancellationTokenSource();

        public HotKey(KeyModifier modifiers, Keys key, int id = 1)
        {
            this.Modifiers = modifiers;
            this.Key = key;
            this.Id = id;
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._Task != null)
                {
                    this._Task.Dispose();
                    this._Task = null;
                }
                if (this.CancellationTokenSource != null)
                {
                    this.CancellationTokenSource.Dispose();
                    this.CancellationTokenSource = null;
                }
            }
        }

        public bool Register()
        {
            EventWaitHandle waitHandle = new AutoResetEvent(false);
            this.MainScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            var result = false;
            this._Task = Task.Factory.StartNew(() =>
                {
                    result = Api.RegisterHotKey(IntPtr.Zero, this.Id, Convert.ToUInt32(this.Modifiers), Convert.ToUInt32(this.Key));
                    waitHandle.Set();
                    Api.MSG msg;
                    sbyte ret;
                    while ((ret = Api.GetMessage(out msg, IntPtr.Zero, 0, 0)) != -1)
                    {
                        if (this.CancellationTokenSource.IsCancellationRequested)
                        {
                            Api.UnregisterHotKey(IntPtr.Zero, this.Id);
                            return;
                        }
                        if (ret == -1)
                            continue;
                        if (msg.message == WindowsMessages.WM_HOTKEY)
                            this.OnHotKeyPressed(new ItemActedEventArgs<int>(msg.wParam.ToInt32()));
                    }
                },
                this.CancellationTokenSource.Token);
            waitHandle.WaitOne();
            return result;
        }

        public void Unregister() { this.CancellationTokenSource.Cancel(); }
        protected virtual void OnHotKeyPressed(ItemActedEventArgs<int> e) { this.HotkeyPressed.RaiseAsync(this, e, this.MainScheduler); }

        public static HotKey Create(KeyModifier keyModifiers, Keys key, Action<int> onHotKeyPressed, int id = 1)
        {
            if (onHotKeyPressed == null)
                throw new ArgumentNullException(nameof(onHotKeyPressed));
            return Create(keyModifiers, key, (_, e) => onHotKeyPressed(e.Item), id);
        }

        public static HotKey Create(KeyModifier keyModifiers, Keys key, Action onHotKeyPressed, int id = 1)
        {
            if (onHotKeyPressed == null)
                throw new ArgumentNullException(nameof(onHotKeyPressed));
            return Create(keyModifiers, key, (_, __) => onHotKeyPressed(), id);
        }

        public static HotKey Create(KeyModifier keyModifiers, Keys key, EventHandler<ItemActedEventArgs<int>> onHotKeyPressed, int id = 1)
        {
            if (onHotKeyPressed == null)
                throw new ArgumentNullException(nameof(onHotKeyPressed));
            var result = new HotKey(keyModifiers, key, id);
            result.HotkeyPressed += onHotKeyPressed;
            if (result.Register())
                return result;
            result.Dispose();
            return null;
        }

        ~HotKey() { this.Dispose(false); }
        public event EventHandler<ItemActedEventArgs<int>> HotkeyPressed;

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}