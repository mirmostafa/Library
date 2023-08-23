using System.Windows.Interop;

namespace Library.Wpf.Dialogs;

internal class Win32WindowForm(IntPtr handle) : System.Windows.Forms.IWin32Window
{
    public Win32WindowForm(Window window)
        : this(new WindowInteropHelper(window).Handle) { }

    public IntPtr Handle { get; } = handle;
}