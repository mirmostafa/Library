using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Mohammad.Win32.Natives
{
    public static partial class Api
    {
        /// <summary>
        ///     Netapi32.dll : The NetServerEnum function lists all servers
        ///     of the specified type that are visible in a domain. For example, an
        ///     application can call NetServerEnum to list all domain controllers only
        ///     or all SQL servers only.
        ///     You can combine bit masks to list several types. For example, a value
        ///     of 0x00000003  combines the bit masks for SV_TYPE_WORKSTATION
        ///     (0x00000001) and SV_TYPE_SERVER (0x00000002)
        /// </summary>
        [DllImport("Netapi32", CharSet = CharSet.Auto, SetLastError = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern uint NetServerEnum(IntPtr serverName, uint level, ref IntPtr siPtr, uint prefmaxlen, ref uint entriesread, ref uint totalentries,
            uint servertype, [MarshalAs(UnmanagedType.LPWStr)] string domain, IntPtr resumeHandle);

        /// <summary>
        ///     Netapi32.dll : The NetApiBufferFree function frees
        ///     the memory that the NetApiBufferAllocate function allocates.
        ///     Call NetApiBufferFree to free the memory that other network
        ///     management functions return.
        /// </summary>
        [DllImport("Netapi32", SetLastError = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int NetApiBufferFree(IntPtr pBuf);

        /// <summary>
        ///     Windows NT/2000/XP Only
        /// </summary>
        [DllImport("netapi32.dll", EntryPoint = "NetServerGetInfo")]
        public static extern uint NetServerGetInfo([MarshalAs(UnmanagedType.LPWStr)] string serverName, int level, ref IntPtr buffPtr);
    }
}