using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Interop;
using Mohammad.EventsArgs;
using Mohammad.Win32.Natives;

namespace Mohammad.Wpf.Windows.Input
{
    public class WindowsHotKey : IDisposable
    {
        private bool _Disposed;
        private int? _Id;
        private const int WM_HOT_KEY = 0x0312;
        private static Dictionary<int, WindowsHotKey> _DictHotKeyToCalBackProc;
        public Key Key { get; private set; }
        public KeyModifier KeyModifiers { get; private set; }
        public int Id { get { return (int) (this._Id ?? (this._Id = KeyInterop.VirtualKeyFromKey(this.Key))); } set { this._Id = value; } }

        public WindowsHotKey(Key key, KeyModifier keyModifiers)
            : this(key, keyModifiers, null, (Action<int>) null) {}

        public WindowsHotKey(Key key, KeyModifier keyModifiers, EventHandler<ItemActedEventArgs<int>> onHotkeyPressed)
            : this(key, keyModifiers, null, onHotkeyPressed) {}

        public WindowsHotKey(Key key, KeyModifier keyModifiers, int? id)
            : this(key, keyModifiers, id, (Action<int>) null) {}

        public WindowsHotKey(Key key, KeyModifier keyModifiers, int? id, EventHandler<ItemActedEventArgs<int>> onHotkeyPressed)
        {
            this.Initialize(key, keyModifiers, id, onHotkeyPressed);
        }

        public WindowsHotKey(Key key, KeyModifier keyModifiers, Action<int> onHotkeyPressed)
            : this(key, keyModifiers, null, onHotkeyPressed) {}

        public WindowsHotKey(Key key, KeyModifier keyModifiers, int? id, Action<int> onHotkeyPressed)
        {
            this.Initialize(key, keyModifiers, id, (_, e) => onHotkeyPressed(e.Item));
        }

        public WindowsHotKey(Key key, KeyModifier keyModifiers, Action onHotkeyPressed)
            : this(key, keyModifiers, null, onHotkeyPressed) {}

        public WindowsHotKey(Key key, KeyModifier keyModifiers, int? id, Action onHotkeyPressed)
        {
            this.Key = key;
            this.KeyModifiers = keyModifiers;
            if (id.HasValue)
                this.Id = id.Value;
            if (onHotkeyPressed != null)
                this.HotkeyPressed += (_, e) => onHotkeyPressed();
        }

        private void Initialize(Key key, KeyModifier keyModifiers, int? id, EventHandler<ItemActedEventArgs<int>> onHotkeyPressed)
        {
            this.Key = key;
            this.KeyModifiers = keyModifiers;
            if (id.HasValue)
                this.Id = id.Value;
            if (onHotkeyPressed != null)
                this.HotkeyPressed += onHotkeyPressed;
        }

        public event EventHandler<ItemActedEventArgs<int>> HotkeyPressed;

        public bool Register()
        {
            var virtualKeyCode = KeyInterop.VirtualKeyFromKey(this.Key);
            var result = Api.RegisterHotKey(IntPtr.Zero, this.Id, (uint) this.KeyModifiers, (uint) virtualKeyCode);

            if (_DictHotKeyToCalBackProc == null)
            {
                _DictHotKeyToCalBackProc = new Dictionary<int, WindowsHotKey>();
                ComponentDispatcher.ThreadFilterMessage += ComponentDispatcherThreadFilterMessage;
            }

            _DictHotKeyToCalBackProc.Add(this.Id, this);

            Debug.Print(result + ", " + this.Id + ", " + virtualKeyCode);
            return result;
        }

        public void Unregister()
        {
            WindowsHotKey windowsHotKey;
            if (_DictHotKeyToCalBackProc.TryGetValue(this.Id, out windowsHotKey))
                Api.UnregisterHotKey(IntPtr.Zero, this.Id);
        }

        private static void ComponentDispatcherThreadFilterMessage(ref MSG msg, ref bool handled)
        {
            if (handled)
                return;
            if (msg.message != WM_HOT_KEY)
                return;
            WindowsHotKey hotKey;
            if (!_DictHotKeyToCalBackProc.TryGetValue((int) msg.wParam, out hotKey))
                return;

            hotKey.HotkeyPressed?.Invoke(hotKey, new ItemActedEventArgs<int>(hotKey.Id));
            handled = true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this._Disposed)
                return;
            if (disposing)
                this.Unregister();
            this._Disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}