using System;
using System.Runtime.InteropServices;

namespace Mohammad.Net.Internals
{
    public class User32Utils
    {
        public static readonly IntPtr HWND_BROADCAST = new IntPtr(0xffff);
        public static readonly IntPtr WM_SETTINGCHANGE = new IntPtr(0x001A);
        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //public static extern int SendMessage(int hWnd, int msg, int wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessageTimeout(IntPtr hWnd, uint Msg, UIntPtr wParam, UIntPtr lParam, SendMessageTimeoutFlags fuFlags, uint uTimeout,
            out UIntPtr lpdwResult);

        public static void Notify_SettingChange()
        {
            UIntPtr result;
            SendMessageTimeout(HWND_BROADCAST, (uint) WM_SETTINGCHANGE, UIntPtr.Zero, UIntPtr.Zero, SendMessageTimeoutFlags.SMTO_NORMAL, 1000, out result);
        }

        private enum SendMessageTimeoutFlags : uint
        {
            SMTO_NORMAL = 0x0000,
            SMTO_BLOCK = 0x0001,
            SMTO_ABORTIFHUNG = 0x0002,
            SMTO_NOTIMEOUTIFNOTHUNG = 0x0008
        }
    }
}