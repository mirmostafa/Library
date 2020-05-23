using System;
using System.Runtime.InteropServices;

namespace Mohammad.Wpf.Internals.Interop
{
    /// <summary>
    ///     Callback delegate which is used by the Windows API to
    ///     submit window messages.
    /// </summary>
    public delegate long WindowProcedureHandler(IntPtr hwnd, uint uMsg, uint wparam, uint lparam);

    /// <summary>
    ///     Win API WNDCLASS struct - represents a single window.
    ///     Used to receive window messages.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct WindowClass
    {
        public int cbClsExtra;
        public int cbWndExtra;
        public IntPtr hbrBackground;
        public IntPtr hCursor;
        public IntPtr hIcon;
        public IntPtr hInstance;
        public WindowProcedureHandler lpfnWndProc;

        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpszClassName;

        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpszMenuName;

        public uint style;
    }
}