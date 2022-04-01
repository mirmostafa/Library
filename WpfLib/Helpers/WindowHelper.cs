using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace Library.Wpf.Helpers;

public static class WindowHelper
{
    [return: NotNull]
    public static TService Service<TService>(this Window? window) =>
    DI.GetService<TService>().NotNull();

    // this code is taken from a sample application provided by Rafael Rivera
    // see the full code sample here: (2016/08)
    // https://github.com/riverar/sample-win10-aeroglass
    public static void EnableBlur(Window win, bool enable = true)
    {
        var windowHelper = new WindowInteropHelper(win);

        var accent = new AccentPolicy
        {
            AccentState = enable ? AccentState.AccentEnableBlurbehind : AccentState.AccentDisabled
        };

        var accentStructSize = Marshal.SizeOf(accent);

        var accentPtr = Marshal.AllocHGlobal(accentStructSize);
        Marshal.StructureToPtr(accent, accentPtr, false);

        var data = new WindowCompositionAttributeData
        {
            Attribute = WindowCompositionAttribute.WcaAccentPolicy,
            SizeOfData = accentStructSize,
            Data = accentPtr
        };

        _ = SetWindowCompositionAttribute(windowHelper.Handle, ref data);

        Marshal.FreeHGlobal(accentPtr);
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct AccentPolicy
    {
        public AccentState AccentState;
        public int AccentFlags;
        public int GradientColor;
        public int AnimationId;
    }
    internal enum AccentState
    {
        AccentDisabled = 0,
        AccentEnableGradient = 1,
        AccentEnableTransparentgradient = 2,
        AccentEnableBlurbehind = 3,
        AccentInvalidState = 4
    }
    [StructLayout(LayoutKind.Sequential)]
    internal struct WindowCompositionAttributeData
    {
        public WindowCompositionAttribute Attribute;
        public IntPtr Data;
        public int SizeOfData;
    }
    internal enum WindowCompositionAttribute
    {
        // ...
        WcaAccentPolicy = 19
        // ...
    }
    [DllImport("user32.dll")]
    internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);
}
