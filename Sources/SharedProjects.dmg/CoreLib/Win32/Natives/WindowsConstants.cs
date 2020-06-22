using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Mohammad.Win32.Natives
{
    public static class WindowsConstants
    {
        public const uint SV_TYPE_AFP = 0x00000040;
        public const uint SV_TYPE_ALL = 0xFFFFFFFF; /* handy for NetServerEnum2 */
        public const uint SV_TYPE_ALTERNATE_XPORT = 0x20000000; /* return list for alternate transport */
        public const uint SV_TYPE_BACKUP_BROWSER = 0x00020000;
        public const uint SV_TYPE_CLUSTER_NT = 0x01000000; /* NT Cluster */
        public const uint SV_TYPE_CLUSTER_VS_NT = 0x04000000; /* NT Cluster Virtual Server Name */
        public const uint SV_TYPE_DCE = 0x10000000; /* IBM DSS (Directory and Security Services) or equivalent */
        public const uint SV_TYPE_DFS = 0x00800000; /* Root of a DFS tree */
        public const uint SV_TYPE_DIALIN_SERVER = 0x00000400;
        public const uint SV_TYPE_DOMAIN_BAKCTRL = 0x00000010;
        public const uint SV_TYPE_DOMAIN_CTRL = 0x00000008;
        public const uint SV_TYPE_DOMAIN_ENUM = 0x80000000;
        public const uint SV_TYPE_DOMAIN_MASTER = 0x00080000;
        public const uint SV_TYPE_DOMAIN_MEMBER = 0x00000100;
        public const uint SV_TYPE_LOCAL_LIST_ONLY = 0x40000000; /* Return local list only */
        public const uint SV_TYPE_MASTER_BROWSER = 0x00040000;
        public const uint SV_TYPE_NOVELL = 0x00000080;
        public const uint SV_TYPE_NT = 0x00001000;
        public const uint SV_TYPE_POTENTIAL_BROWSER = 0x00010000;
        public const uint SV_TYPE_PRuintQ_SERVER = 0x00000200;
        public const uint SV_TYPE_SERVER = 0x00000002;
        public const uint SV_TYPE_SERVER_MFPN = 0x00004000;
        public const uint SV_TYPE_SERVER_NT = 0x00008000;
        public const uint SV_TYPE_SERVER_OSF = 0x00100000;
        public const uint SV_TYPE_SERVER_UNIX = SV_TYPE_XENIX_SERVER;
        public const uint SV_TYPE_SERVER_VMS = 0x00200000;
        public const uint SV_TYPE_SQLSERVER = 0x00000004;
        public const uint SV_TYPE_TERMINALSERVER = 0x02000000; /* Terminal Server(Hydra) */
        public const uint SV_TYPE_TIME_SOURCE = 0x00000020;
        public const uint SV_TYPE_WFW = 0x00002000;
        public const uint SV_TYPE_WINDOWS = 0x00400000; /* Windows95 and above */
        public const uint SV_TYPE_WORKSTATION = 0x00000001;
        public const uint SV_TYPE_XENIX_SERVER = 0x00000800;
        public const int GENERIC_ALL_ACCESS = 0x10000000;
        public const int PrivilegeEnabled = 0x00000002;
        public const int TokenQuery = 0x00000008;
        public const int AdjustPrivileges = 0x00000020;
        public const string ShutdownPrivilege = "SeShutdownPrivilege";
        public const int LOGON32_LOGON_INTERACTIVE = 2;
        public const int LOGON32_LOGON_NETWORK = 3;
        public const int LOGON32_LOGON_BATCH = 4;
        public const int LOGON32_LOGON_SERVICE = 5;
        public const int LOGON32_LOGON_UNLOCK = 7;
        public const int LOGON32_LOGON_NETWORK_CLEARTEXT = 8;
        public const int LOGON32_LOGON_NEW_CREDENTIALS = 9;
        public const int LOGON32_PROVIDER_DEFAULT = 0;
        public const int LOGON32_PROVIDER_WINNT35 = 1;
        public const int LOGON32_PROVIDER_WINNT40 = 2;
        public const int LOGON32_PROVIDER_WINNT50 = 3;
        public const int MOD_ALT = 0x0001;
        public const int MOD_CONTROL = 0x0002;
        public const int MOD_SHIFT = 0x0004;
        public const int MOD_WIN = 0x0008;
        public const int MOD_NOREPEAT = 0x4000;
        public const int GW_HWNDNEXT = 2;
        public const int GW_HWNDPREV = 3;
        public const int GW_CHILD = 5;
        public const int MF_BYPOSITION = 0x400;
        public const int FILE_NOTIFY_CHANGE_ATTRIBUTES = 0x00000004;
        public const int FILE_NOTIFY_CHANGE_SIZE = 0x00000008;
        public const int FILE_NOTIFY_CHANGE_LAST_ACCESS = 0x00000020;
        public const int FILE_NOTIFY_CHANGE_CREATION = 0x00000040;
        public const int FILE_NOTIFY_CHANGE_SECURITY = 0x00000100;
        public const uint INFINITE = 0xFFFFFFFF;
        public const uint WAIT_ABANDONED = 0x00000080;
        public const uint WAIT_OBJECT_0 = 0x00000000;
        public const uint WAIT_TIMEOUT = 0x00000102;
        public const uint FILE_LIST_DIRECTORY = 0x1;
        public const uint FILE_SHARE_READ = 0x1;
        public const uint FILE_SHARE_WRITE = 0x2;
        public const uint FILE_SHARE_DELETE = 0x4;
        public const uint OPEN_EXISTING = 3;
        public const uint FILE_FLAG_BACKUP_SEMANTICS = 0x2000000;
        public const uint FILE_NOTIFY_CHANGE_FILE_NAME = 0x1;
        public const uint FILE_NOTIFY_CHANGE_DIR_NAME = 0x2;
        public const uint FILE_NOTIFY_CHANGE_LAST_WRITE = 0x10;
        public const int BIF_RETURNONLYFSDIRS = 0x00000001;
        public const int BIF_DONTGOBELOWDOMAIN = 0x00000002;
        public const int BIF_STATUSTEXT = 0x00000004;
        public const int BIF_RETURNFSANCESTORS = 0x00000008;
        public const int BIF_EDITBOX = 0x00000010;
        public const int BIF_VALIDATE = 0x00000020;
        public const int BIF_NEWDIALOGSTYLE = 0x00000040;
        public const int BIF_BROWSEINCLUDEURLS = 0x00000080;
        public const int BIF_USENEWUI = BIF_EDITBOX | BIF_NEWDIALOGSTYLE;
        public const int BIF_UAHINT = 0x00000100;
        public const int BIF_NONEWFOLDERBUTTON = 0x00000200;
        public const int BIF_NOTRANSLATETARGETS = 0x00000400;
        public const int BIF_BROWSEFORCOMPUTER = 0x00000800;
        public const int BIF_BROWSEFORPRINTER = 0x00002000;
        public const int BIF_BROWSEINCLUDEFILES = 0x00004000;
        public const int BIF_SHAREABLE = 0x00008000;
        public const int BIF_BROWSEFILEJUNCTIONS = 0x00010000;
        public const int BFFM_INITIALIZED = 1;
        public const int BFFM_SELCHANGED = 2;
        public const int BFFM_VALIDATEFAILEDA = 3;
        public const int BFFM_VALIDATEFAILEDW = 4;
        public const int BFFM_IUNKNOWN = 5;
        public const int BFFM_SETSTATUSTEXTA = 0x0400 + 100;
        public const int BFFM_ENABLEOK = 0x0400 + 101;
        public const int BFFM_SETSELECTIONA = 0x0400 + 102;
        public const int BFFM_SETSELECTIONW = 0x0400 + 103;
        public const int BFFM_SETSTATUSTEXTW = 0x0400 + 104;
        public const int BFFM_SETOKTEXT = 0x0400 + 105;
        public const int BFFM_SETEXPANDED = 0x0400 + 106;
        public const int CSIDL_WINDOWS = 0x24;
        public const int SW_HIDE = 0;
        public const int SW_SHOW = 5;
        public const int SHGDN_NORMAL = 0x0000;
        public const int SHGDN_INFOLDER = 0x0001;
        public const int SHGDN_FOREDITING = 0x1000;
        public const int SHGDN_FORADDRESSBAR = 0x4000;
        public const int SHGDN_FORPARSING = 0x8000;
    }

    [Flags]
    public enum KeyModifier
    {
        None = 0x0000,
        Alt = 0x0001,
        Ctrl = 0x0002,
        NoRepeat = 0x4000,
        Shift = 0x0004,
        Win = 0x0008
    }

    public delegate int BrowseCallBackProc(IntPtr hwnd, int msg, IntPtr lp, IntPtr wp);

    public struct BROWSEINFO
    {
        /// <summary>
        ///     A handle to the owner window for the dialog box.
        /// </summary>
        public IntPtr hwndOwner;

        /// <summary>
        ///     An integer value that receives the index of the image associated with the selected folder, stored in the system
        ///     image list.
        /// </summary>
        public int iImage;

        /// <summary>
        ///     An application-defined value that the dialog box passes to the callback function, if one is specified in lpfn.
        /// </summary>
        public IntPtr lParam;

        /// <summary>
        ///     Pointer to an application-defined function that the dialog box calls when an event occurs. For more information,
        ///     see the BrowseCallbackProc function. This member can be NULL.
        /// </summary>
        public BrowseCallBackProc lpfn;

        /// <summary>
        ///     Pointer to a null-terminated string that is displayed above the tree view control in the dialog box. This string
        ///     can be used to specify instructions to the user.
        /// </summary>
        public string lpszTitle;

        /// <summary>
        ///     Type: PCIDLIST_ABSOLUTE
        ///     A PIDL that specifies the location of the root folder from which to start browsing. Only the specified folder and
        ///     its subfolders in the namespace hierarchy appear in the dialog box. This member can be NULL; in that case, the
        ///     namespace root (the Desktop folder) is used.
        /// </summary>
        public IntPtr pidlRoot;

        public string pszDisplayName;

        /// <summary>
        ///     Flags that specify the options for the dialog box. This member can be 0 or a combination of the following values.
        ///     Version numbers refer to the minimum version of Shell32.dll required for SHBrowseForFolder to recognize flags added
        ///     in later releases.
        /// </summary>
        public uint ulFlags;
    }

    [ComImport]
    [Guid("00000002-0000-0000-c000-000000000046")]
    [SuppressUnmanagedCodeSecurity]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMalloc
    {
        IntPtr Alloc(int cb);
        void Free(IntPtr pv);
        IntPtr Realloc(IntPtr pv, int cb);
        int GetSize(IntPtr pv);
        int DidAlloc(IntPtr pv);
        void HeapMinimize();
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("000214E6-0000-0000-C000-000000000046")]
    public interface IShellFolder
    {
        // Translates a file object's or folder's display name into an item identifier list.
        // Return value: error code, if any
        [PreserveSig]
        int ParseDisplayName(IntPtr hwnd,
            // Optional window handle
            IntPtr pbc,
            // Optional bind context that controls the parsing operation. This parameter is 
            // normally set to NULL. 
            [MarshalAs(UnmanagedType.LPWStr)] string pszDisplayName,
            // Null-terminated UNICODE string with the display name.
            ref uint pchEaten,
            // Pointer to a ULONG value that receives the number of characters of the 
            // display name that was parsed.
            out IntPtr ppidl,
            // Pointer to an ITEMIDLIST pointer that receives the item identifier list for 
            // the object.
            ref uint pdwAttributes); // Optional parameter that can be used to query for file attributes.
        // this can be values from the SFGAO enum

        // Allows a client to determine the contents of a folder by creating an item identifier enumeration object 
        // and returning its IEnumIDList interface.
        // Return value: error code, if any
        [PreserveSig]
        int EnumObjects(IntPtr hwnd,
            // If user input is required to perform the enumeration, this window handle 
            // should be used by the enumeration object as the parent window to take 
            // user input.
            int grfFlags,
            // Flags indicating which items to include in the enumeration. For a list 
            // of possible values, see the SHCONTF enum. 
            out IntPtr ppenumIDList); // Address that receives a pointer to the IEnumIDList interface of the 
        // enumeration object created by this method. 

        // Retrieves an IShellFolder object for a subfolder.
        // Return value: error code, if any
        [PreserveSig]
        int BindToObject(IntPtr pidl,
            // Address of an ITEMIDLIST structure (PIDL) that identifies the subfolder.
            IntPtr pbc,
            // Optional address of an IBindCtx interface on a bind context object to be 
            // used during this operation.
            Guid riid,
            // Identifier of the interface to return. 
            out IntPtr ppv); // Address that receives the interface pointer.

        // Requests a pointer to an object's storage interface. 
        // Return value: error code, if any
        [PreserveSig]
        int BindToStorage(IntPtr pidl,
            // Address of an ITEMIDLIST structure that identifies the subfolder relative 
            // to its parent folder. 
            IntPtr pbc,
            // Optional address of an IBindCtx interface on a bind context object to be 
            // used during this operation.
            Guid riid,
            // Interface identifier (IID) of the requested storage interface.
            out IntPtr ppv); // Address that receives the interface pointer specified by riid.

        // Determines the relative order of two file objects or folders, given their item identifier lists.
        // Return value: If this method is successful, the CODE field of the HRESULT contains one of the following 
        // values (the code can be retrived using the helper function GetHResultCode):
        // Negative A negative return value indicates that the first item should precede the second (pidl1 < pidl2). 
        // Positive A positive return value indicates that the first item should follow the second (pidl1 > pidl2). 
        // Zero A return value of zero indicates that the two items are the same (pidl1 = pidl2). 
        [PreserveSig]
        int CompareIDs(int lParam,
            // Value that specifies how the comparison should be performed. The lower 
            // sixteen bits of lParam define the sorting rule. The upper sixteen bits of 
            // lParam are used for flags that modify the sorting rule. values can be from 
            // the SHCIDS enum
            IntPtr pidl1,
            // Pointer to the first item's ITEMIDLIST structure.
            IntPtr pidl2); // Pointer to the second item's ITEMIDLIST structure.

        // Requests an object that can be used to obtain information from or interact with a folder object.
        // Return value: error code, if any
        [PreserveSig]
        int CreateViewObject(IntPtr hwndOwner,
            // Handle to the owner window.
            Guid riid,
            // Identifier of the requested interface. 
            out IntPtr ppv); // Address of a pointer to the requested interface. 

        // Retrieves the attributes of one or more file objects or subfolders. 
        // Return value: error code, if any
        [PreserveSig]
        int GetAttributesOf(uint cidl,
            // Number of file objects from which to retrieve attributes. 

            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] IntPtr[] apidl,
            // Address of an array of pointers to ITEMIDLIST structures, each of which 
            // uniquely identifies a file object relative to the parent folder.
            ref uint rgfInOut); // Address of a single ULONG value that, on entry, contains the attributes that 
        // the caller is requesting. On exit, this value contains the requested 
        // attributes that are common to all of the specified objects. this value can
        // be from the SFGAO enum

        // Retrieves an OLE interface that can be used to carry out actions on the specified file objects or folders.
        // Return value: error code, if any
        [PreserveSig]
        int GetUIObjectOf(IntPtr hwndOwner,
            // Handle to the owner window that the client should specify if it displays 
            // a dialog box or message box.
            uint cidl,
            // Number of file objects or subfolders specified in the apidl parameter. 
            IntPtr[] apidl,
            // Address of an array of pointers to ITEMIDLIST structures, each of which 
            // uniquely identifies a file object or subfolder relative to the parent folder.
            Guid riid,
            // Identifier of the COM interface object to return.
            ref uint rgfReserved,
            // Reserved. 
            out IntPtr ppv); // Pointer to the requested interface.

        // Retrieves the display name for the specified file object or subfolder. 
        // Return value: error code, if any
        [PreserveSig]
        int GetDisplayNameOf(IntPtr pidl,
            // Address of an ITEMIDLIST structure (PIDL) that uniquely identifies the file 
            // object or subfolder relative to the parent folder. 
            uint uFlags,
            // Flags used to request the type of display name to return. For a list of 
            // possible values, see the SHGNO enum. 
            out STRRET pName); // Address of a STRRET structure in which to return the display name.

        // Sets the display name of a file object or subfolder, changing the item identifier in the process.
        // Return value: error code, if any
        [PreserveSig]
        int SetNameOf(IntPtr hwnd,
            // Handle to the owner window of any dialog or message boxes that the client 
            // displays.
            IntPtr pidl,
            // Pointer to an ITEMIDLIST structure that uniquely identifies the file object
            // or subfolder relative to the parent folder. 
            [MarshalAs(UnmanagedType.LPWStr)] string pszName,
            // Pointer to a null-terminated string that specifies the new display name. 
            uint uFlags,
            // Flags indicating the type of name specified by the lpszName parameter. For 
            // a list of possible values, see the description of the SHGNO enum. 
            out IntPtr ppidlOut); // Address of a pointer to an ITEMIDLIST structure which receives the new ITEMIDLIST. 
    }

    [StructLayout(LayoutKind.Explicit, Size = 264)]
    public struct STRRET
    {
        [FieldOffset(0)]
        public uint uType; // One of the STRRET_* values

        [FieldOffset(4)]
        public IntPtr pOleStr; // must be freed by caller of GetDisplayNameOf

        [FieldOffset(4)]
        public IntPtr pStr; // NOT USED

        [FieldOffset(4)]
        public uint uOffset; // Offset into SHITEMID

        [FieldOffset(4)]
        public IntPtr cStr; // Buffer to fill in (ANSI)
    }
}