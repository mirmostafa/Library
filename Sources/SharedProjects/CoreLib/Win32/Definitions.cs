using System;
using Mohammad.Win32.Natives;

namespace Mohammad.Win32
{
    [Flags]
    public enum WindowAnimateFlags : long
    {
        HorizentalPositive = 0x00000001,
        HorizentalNegative = 0x00000002,
        VerticalPositive = 0x00000004,
        VerticalNegative = 0x00000008,
        Center = 0x00000010,
        Hide = 0x00010000,
        Activate = 0x00020000,
        Slide = 0x00040000,
        Blend = 0x00080000
    }

    public enum LogonType
    {
        Interactive = WindowsConstants.LOGON32_LOGON_INTERACTIVE,
        Network = WindowsConstants.LOGON32_LOGON_NETWORK,
        Batch = WindowsConstants.LOGON32_LOGON_BATCH,
        Service = WindowsConstants.LOGON32_LOGON_SERVICE,
        Unlock = WindowsConstants.LOGON32_LOGON_UNLOCK,
        NetworkClearText = WindowsConstants.LOGON32_LOGON_NETWORK_CLEARTEXT,
        NewCredentials = WindowsConstants.LOGON32_LOGON_NEW_CREDENTIALS
    }

    public enum LogonProvider
    {
        Default = WindowsConstants.LOGON32_PROVIDER_DEFAULT,
        WinNt35 = WindowsConstants.LOGON32_PROVIDER_WINNT35,
        WinNt40 = WindowsConstants.LOGON32_PROVIDER_WINNT40,
        WinNt50 = WindowsConstants.LOGON32_PROVIDER_WINNT50
    }
}