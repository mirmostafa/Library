using System;
using System.Runtime.InteropServices;
using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

namespace Mohammad.Net.Internals
{
    internal class WinINetUtils
    {
        private const uint INTERNET_PER_CONN_PROXY_SERVER = 2;
        private const uint INTERNET_PER_CONN_PROXY_BYPASS = 3;
        private const uint INTERNET_PER_CONN_FLAGS = 1;

        private const uint INTERNET_OPTION_REFRESH = 37;
        private const uint INTERNET_OPTION_PROXY = 38;
        private const uint INTERNET_OPTION_SETTINGS_CHANGED = 39;
        private const uint INTERNET_OPTION_END_BROWSER_SESSION = 42;
        private const uint INTERNET_OPTION_PER_CONNECTION_OPTION = 75;

        private const uint PROXY_TYPE_DIRECT = 0x1;
        private const uint PROXY_TYPE_PROXY = 0x2;

        private const uint INTERNET_OPEN_TYPE_PROXY = 3;

        [DllImport("wininet.dll", EntryPoint = "InternetSetOptionA", CharSet = CharSet.Ansi, SetLastError = true, PreserveSig = true)]
        private static extern bool InternetSetOption(IntPtr hInternet, uint dwOption, IntPtr pBuffer, int dwReserved);

        internal static void Notify_OptionSettingChanges()
        {
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct INTERNET_CONNECTED_INFO
        {
            private readonly int dwConnectedState;
            private readonly int dwFlags;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct INTERNET_PER_CONN_OPTION
        {
            private readonly uint dwOption;
            private readonly Value1 Value;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct INTERNET_PER_CONN_OPTION_LIST
        {
            private readonly uint dwSize;

            [MarshalAs(UnmanagedType.LPStr, SizeConst = 256)]
            private readonly string pszConnection;

            private readonly uint dwOptionCount;
            private readonly uint dwOptionError;
            private readonly IntPtr pOptions;
        }

        private struct Value1
        {
            private uint dwValue;
            private FILETIME ftValue;
            private string pszValue;
        }
    }
}