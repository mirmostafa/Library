using System.Windows;
using System.Windows.Interop;
using Mohammad.Wpf.Internals;

namespace Mohammad.Wpf.Windows
{
    public class Windows7Tools
    {
        internal readonly WindowInteropHelper Interop;
        private Taskbar _Taskbar;
        public Taskbar Taskbar => this._Taskbar ?? (this._Taskbar = new Taskbar(this));
        internal Window Window { get; }

        public Windows7Tools(Window window)
        {
            this.Window = window;
            this.Interop = new WindowInteropHelper(this.Window);
        }
    }
}