using System;

namespace Mohammad.Win32.Natives.IfacesEnumsStructsClasses
{
    [Flags]
    public enum ConnectionState
    {
        /// <summary>
        ///     Local system uses a modem to connect to the Internet.
        /// </summary>
        INTERNET_CONNECTION_MODEM = 0x1,

        /// <summary>
        ///     Local system uses a local area network to connect to the Internet.
        /// </summary>
        INTERNET_CONNECTION_LAN = 0x2,

        /// <summary>
        ///     Local system uses a proxy server to connect to the Internet.
        /// </summary>
        INTERNET_CONNECTION_PROXY = 0x4,

        /// <summary>
        ///     Local system has RAS installed.
        /// </summary>
        INTERNET_RAS_INSTALLED = 0x10,

        /// <summary>
        ///     Local system is in offline mode.
        /// </summary>
        INTERNET_CONNECTION_OFFLINE = 0x20,

        /// <summary>
        ///     Local system has a valid connection to the Internet, but it might or might not be currently connected.
        /// </summary>
        INTERNET_CONNECTION_CONFIGURED = 0x40
    }

    public enum ShowCommands
    {
        SW_HIDE = 0,
        SW_SHOWNORMAL = 1,
        SW_NORMAL = 1,
        SW_SHOWMINIMIZED = 2,
        SW_SHOWMAXIMIZED = 3,
        SW_MAXIMIZE = 3,
        SW_SHOWNOACTIVATE = 4,
        SW_SHOW = 5,
        SW_MINIMIZE = 6,
        SW_SHOWMINNOACTIVE = 7,
        SW_SHOWNA = 8,
        SW_RESTORE = 9,
        SW_SHOWDEFAULT = 10,
        SW_FORCEMINIMIZE = 11,
        SW_MAX = 11
    }

    public enum ShellExecuteVerbs
    {
        OpenFile,
        EditFile,
        ExploreFolder,
        FindInFolder,
        PrintFile
    }

    /// <summary>
    ///     The possible flag values for Server Type (see lmserver.h).
    /// </summary>
    [Flags]
    public enum ServerType : uint
    {
        /// <summary>
        ///     Opposite of All.  No servers will be returned.
        /// </summary>
        None = 0x00000000,

        /// <summary>
        ///     All workstations
        /// </summary>
        Workstation = 0x00000001,

        /// <summary>
        ///     All servers
        /// </summary>
        Server = 0x00000002,

        /// <summary>
        ///     Any server running with Microsoft SQL Server
        /// </summary>
        SQLServer = 0x00000004,

        /// <summary>
        ///     Primary domain controller
        /// </summary>
        DomainController = 0x00000008,

        /// <summary>
        ///     Backup domain controller
        /// </summary>
        DomainBackupController = 0x00000010,

        /// <summary>
        ///     Server running the Timesource service
        /// </summary>
        TimeSource = 0x00000020,

        /// <summary>
        ///     Apple File Protocol servers
        /// </summary>
        AFP = 0x00000040,

        /// <summary>
        ///     Novell servers
        /// </summary>
        Novell = 0x00000080,

        /// <summary>
        ///     LAN Manager 2.x domain member
        /// </summary>
        DomainMember = 0x00000100,

        /// <summary>
        ///     Server sharing print queue
        /// </summary>
        PrintQueue = 0x00000200,

        /// <summary>
        ///     Server running dial-in service
        /// </summary>
        Dialin = 0x00000400,

        /// <summary>
        ///     Xenix server
        /// </summary>
        Xenix = 0x00000800,

        /// <summary>
        ///     Unix servers?
        /// </summary>
        Unix = Xenix,

        /// <summary>
        ///     Windows NT workstation or server
        /// </summary>
        NT = 0x00001000,

        /// <summary>
        ///     Server running Windows for Workgroups
        /// </summary>
        WFW = 0x00002000,

        /// <summary>
        ///     Microsoft File and Print for NetWare
        /// </summary>
        MFPN = 0x00004000,

        /// <summary>
        ///     Server that is not a domain controller
        /// </summary>
        NTServer = 0x00008000,

        /// <summary>
        ///     Server that can run the browser service
        /// </summary>
        PotentialBrowser = 0x00010000,

        /// <summary>
        ///     Server running a browser service as backup
        /// </summary>
        BackupBrowser = 0x00020000,

        /// <summary>
        ///     Server running the master browser service
        /// </summary>
        MasterBrowser = 0x00040000,

        /// <summary>
        ///     Server running the domain master browser
        /// </summary>
        DomainMaster = 0x00080000,

        /// <summary>
        ///     Not documented on MSDN? Help Microsoft!
        /// </summary>
        OSF = 0x00100000,

        /// <summary>
        ///     Running VMS
        /// </summary>
        VMS = 0x00200000,

        /// <summary>
        ///     Windows 95 or later
        /// </summary>
        Windows = 0x00400000,

        /// <summary>
        ///     Distributed File System??
        /// </summary>
        DFS = 0x00800000,

        /// <summary>
        ///     Not documented on MSDN? Help Microsoft!
        /// </summary>
        ClusterNT = 0x01000000,

        /// <summary>
        ///     Terminal Server
        /// </summary>
        TerminalServer = 0x02000000,

        /// <summary>
        ///     Not documented on MSDN? Help Microsoft!
        /// </summary>
        DCE = 0x10000000,

        /// <summary>
        ///     Not documented on MSDN? Help Microsoft!
        /// </summary>
        AlternateXPort = 0x20000000,

        /// <summary>
        ///     Servers maintained by the browser
        /// </summary>
        ListOnly = 0x40000000,

        /// <summary>
        ///     List Domains
        /// </summary>
        DomainEnum = 0x80000000,

        /// <summary>
        ///     All servers
        /// </summary>
        All = 0xFFFFFFFF
    }

    public enum SECURITY_IMPERSONATION_LEVEL
    {
        SecurityAnonymous,
        SecurityIdentification,
        SecurityImpersonation,
        SecurityDelegation
    }

    public enum TOKEN_TYPE
    {
        TokenPrimary = 1,
        TokenImpersonation
    }

    public enum Reason : uint
    {
        ApplicationIssue = 0x00040000,
        HardwareIssue = 0x00010000,
        SoftwareIssue = 0x00030000,
        PlannedShutdown = 0x80000000
    }

    [Flags]
    public enum ExitFlags
    {
        Logoff = 0,
        Shutdown = 1,
        Reboot = 2,
        Force = 4,
        PowerOff = 8,
        ForceIfHung = 16
    }

    [Flags]
    public enum AnimateWindowFlags : uint
    {
        AW_HOR_POSITIVE = 0x00000001,
        AW_HOR_NEGATIVE = 0x00000002,
        AW_VER_POSITIVE = 0x00000004,
        AW_VER_NEGATIVE = 0x00000008,
        AW_CENTER = 0x00000010,
        AW_HIDE = 0x00010000,
        AW_ACTIVATE = 0x00020000,
        AW_SLIDE = 0x00040000,
        AW_BLEND = 0x00080000
    }

    public enum ChildWindowFromPointFlags : uint
    {
        CWP_ALL = 0x0000,
        CWP_SKIPINVISIBLE = 0x0001,
        CWP_SKIPDISABLED = 0x0002,
        CWP_SKIPTRANSPARENT = 0x0004
    }

    public enum GetWindowCommand : uint
    {
        GW_HWNDFIRST = 0,
        GW_HWNDLAST = 1,
        GW_HWNDNEXT = 2,
        GW_HWNDPREV = 3,
        GW_OWNER = 4,
        GW_CHILD = 5,
        GW_ENABLEDPOPUP = 6,
        GW_MAX = 6
    }

    [Flags]
    public enum FormatMessageFlags : uint
    {
        FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100,
        FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200,
        FORMAT_MESSAGE_FROM_STRING = 0x00000400,
        FORMAT_MESSAGE_FROM_HMODULE = 0x00000800,
        FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000,
        FORMAT_MESSAGE_ARGUMENT_ARRAY = 0x00002000,
        FORMAT_MESSAGE_MAX_WIDTH_MASK = 0x000000FF
    }
}