using System;
using System.Runtime.InteropServices;

namespace Mohammad.Win32.Natives
{
    public partial class Api
    {
        //[DllImport("kernel32.dll")]
        //public static extern bool ReadDirectoryChangesW(IntPtr hDirectory,
        //												 IntPtr lpBuffer,
        //												 uint nBufferLength,
        //												 bool bWatchSubtree,
        //												 uint dwNotifyFilter,
        //												 out uint lpBytesReturned,
        //												 IntPtr lpOverlapped,
        //												 LPOVERLAPPED_COMPLETION_ROUTINE lpCompletionRoutine);

        [DllImport("kernel32.dll")]
        public static extern IntPtr FindFirstChangeNotification(string lpPathName, bool bWatchSubtree, uint dwNotifyFilter);

        [DllImport("kernel32.dll")]
        public static extern bool FindNextChangeNotification(IntPtr hChangeHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition,
            uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32.dll", EntryPoint = "CloseHandle", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        public static extern bool ReadDirectoryChangesW(IntPtr hDirectory, IntPtr lpBuffer, uint nBufferLength, bool bWatchSubtree, uint dwNotifyFilter,
            out uint lpBytesReturned, IntPtr lpOverlapped, IntPtr lpCompletionRoutine);
    }
}