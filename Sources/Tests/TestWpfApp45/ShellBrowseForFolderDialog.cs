using System;
using System.Runtime.InteropServices;
using Mohammad.Win32.Natives;

namespace TestWpfApp45
{
    public class ShellBrowseForFolderDialog
    {
        /// <summary> Select the root type </summary>
        private readonly RootTypeOptions _RootType = RootTypeOptions.ByPath;

        /// <summary> valid only if RootType is RootTypeOptions.ByPath </summary>
        private string _RootPath;

        public uint DetailsFlags;

        /// <summary> Handle to the owner window for the dialog box.  </summary>
        public IntPtr hwndOwner;

        /// <summary> valid only if RootType is RootTypeOptions.BySpecialFolder </summary>
        public CSIDL RootSpecialFolder;

        /// <summary>
        ///     Address of a null-terminated string that is displayed above the tree view control in the dialog box.
        /// </summary>
        public string Title;

        /// <summary> Token that can be used to represent a particular user. </summary>
        public IntPtr UserToken;

        /// <summary> Address of a buffer to receive the display name of the folder selected by the user. </summary>
        public string DisplayName { get; private set; }

        /// <summary> Return the result of the dialog </summary>
        public string FullName { get; private set; }

        /// <summary> valid only if RootType is RootTypeOptions.ByPath </summary>
        public string RootPath { get { return this._RootPath; } set { this._RootPath = value; } }

        public ShellBrowseForFolderDialog()
        {
            this.hwndOwner = IntPtr.Zero;
            this._RootType = RootTypeOptions.BySpecialFolder;
            this.RootSpecialFolder = CSIDL.CSIDL_DESKTOP;
            this._RootPath = "";
            this.DisplayName = "";
            this.Title = "";
            this.UserToken = IntPtr.Zero;
            this.FullName = "";

            // Default flags values
            this.DetailsFlags = WindowsConstants.BIF_BROWSEINCLUDEFILES | WindowsConstants.BIF_EDITBOX | WindowsConstants.BIF_NEWDIALOGSTYLE |
                                WindowsConstants.BIF_SHAREABLE | WindowsConstants.BIF_STATUSTEXT | WindowsConstants.BIF_USENEWUI | WindowsConstants.BIF_VALIDATE;
        }

        private static IMalloc GetMalloc()
        {
            IntPtr ptrRet;
            Api.SHGetMalloc(out ptrRet);

            var obj = Marshal.GetTypedObjectForIUnknown(ptrRet, typeof(IMalloc));
            var imalloc = (IMalloc) obj;

            return imalloc;
        }

        public bool ShowDialog()
        {
            this.FullName = "";
            this.DisplayName = "";

            // Get shell's memory allocator, it is needed to free some memory later
            var pMalloc = GetMalloc();

            IntPtr pidlRoot;

            if (this._RootType == RootTypeOptions.BySpecialFolder)
            {
                Api.SHGetFolderLocation(this.hwndOwner, (int) this.RootSpecialFolder, this.UserToken, 0, out pidlRoot);
            }
            else // m_RootType = RootTypeOptions.ByPath
            {
                uint iAttribute;
                Api.SHParseDisplayName(this._RootPath, IntPtr.Zero, out pidlRoot, 0, out iAttribute);
            }

            var bi = new BROWSEINFO
                     {
                         hwndOwner = this.hwndOwner,
                         pidlRoot = pidlRoot,
                         pszDisplayName = new string(' ', 256),
                         lpszTitle = this.Title,
                         ulFlags = this.DetailsFlags,
                         lParam = IntPtr.Zero,
                         lpfn = null
                     };

            var pidlSelected = Api.SHBrowseForFolder(ref bi);

            this.DisplayName = bi.pszDisplayName;
            var isf = GetDesktopFolder();

            STRRET ptrDisplayName;
            isf.GetDisplayNameOf(pidlSelected, WindowsConstants.SHGDN_NORMAL | WindowsConstants.SHGDN_FORPARSING, out ptrDisplayName);

            string sDisplay;
            Api.StrRetToBSTR(ref ptrDisplayName, pidlRoot, out sDisplay);
            this.FullName = sDisplay;

            if (pidlRoot != IntPtr.Zero)
                pMalloc.Free(pidlRoot);

            if (pidlSelected != IntPtr.Zero)
                pMalloc.Free(pidlSelected);

            Marshal.ReleaseComObject(isf);
            Marshal.ReleaseComObject(pMalloc);
            GC.Collect();
            GC.Collect(0);
            GC.Collect(1);
            GC.Collect(2);
            GC.Collect(GC.GetGeneration(this));
            GC.WaitForFullGCComplete();
            return pidlSelected != IntPtr.Zero;
        }

        private static IShellFolder GetDesktopFolder()
        {
            IntPtr ptrRet;
            Api.SHGetDesktopFolder(out ptrRet);

            var shellFolderType = typeof(IShellFolder);
            var obj = Marshal.GetTypedObjectForIUnknown(ptrRet, shellFolderType);
            var ishellFolder = (IShellFolder) obj;

            return ishellFolder;
        }

        private enum RootTypeOptions
        {
            BySpecialFolder,
            ByPath
        }
    }
}