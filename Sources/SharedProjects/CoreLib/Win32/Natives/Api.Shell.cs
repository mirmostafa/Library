using System;
using System.Runtime.InteropServices;
using System.Text;
using Mohammad.Helpers;
using Mohammad.Win32.Natives.IfacesEnumsStructsClasses;

namespace Mohammad.Win32.Natives
{
    partial class Api
    {
        public static IntPtr ShellExecute(IntPtr hwnd, ShellExecuteVerbs lpOperation, string lpFile, string lpParameters, string lpDirectory, ShowCommands nShowCmd)
        {
            string verb;
            switch (lpOperation)
            {
                case ShellExecuteVerbs.OpenFile:
                    verb = "open";
                    break;
                case ShellExecuteVerbs.EditFile:
                    verb = "edit";
                    break;
                case ShellExecuteVerbs.ExploreFolder:
                    verb = "explore";
                    break;
                case ShellExecuteVerbs.FindInFolder:
                    verb = "find";
                    break;
                case ShellExecuteVerbs.PrintFile:
                    verb = "print";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("lpOperation");
            }
            return ShellExecute(hwnd, verb, lpFile, lpParameters, lpDirectory, nShowCmd.ToInt());
        }

        [DllImport("shell32.dll")]
        public static extern IntPtr ShellExecute(IntPtr hwnd, [MarshalAs(UnmanagedType.LPTStr)] string lpOperation, [MarshalAs(UnmanagedType.LPTStr)] string lpFile,
            [MarshalAs(UnmanagedType.LPTStr)] string lpParameters, [MarshalAs(UnmanagedType.LPTStr)] string lpDirectory, int nShowCmd);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern int SHFileOperation(ref SHFILEOPSTRUCT lpFileOp);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr SHBrowseForFolder(ref BROWSEINFO lpbi);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern int SHGetMalloc([Out] [MarshalAs(UnmanagedType.LPArray)] IMalloc[] ppMalloc);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern int SHGetMalloc([Out] [MarshalAs(UnmanagedType.LPArray)] out IMalloc[] ppMalloc);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern int SHGetMalloc([Out] out IntPtr ppMalloc);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern bool SHGetPathFromIDList(IntPtr pidl, IntPtr pszPath);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern int SHGetSpecialFolderLocation(IntPtr hwnd, int csidl, ref IntPtr ppidl);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, string lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Unicode)]
        public static extern IntPtr SendMessage2(HandleRef hWnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetWindowText(int hWnd, StringBuilder text, int count);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern int SHGetFolderLocation(IntPtr hwndOwner, int nFolder, IntPtr hToken, uint dwReserved, out IntPtr ppidl);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern int SHParseDisplayName([MarshalAs(UnmanagedType.LPWStr)] string pszName, IntPtr pbc, out IntPtr ppidl, uint sfgaoIn, out uint psfgaoOut);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern int SHGetDesktopFolder(ref IShellFolder ppshf);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern int SHGetDesktopFolder(out IntPtr ppshf);

        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        public static extern int StrRetToBSTR(ref STRRET pstr, IntPtr pidl, [MarshalAs(UnmanagedType.BStr)] out string pbstr);
    }
}