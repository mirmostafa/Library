using System;
using System.Runtime.InteropServices;

namespace Mohammad.Win32.Natives
{
    partial class Api
    {
        [DllImport("wininet.dll", SetLastError = true)]
        public static extern long FindCloseUrlCache(IntPtr hEnumHandle);

        [DllImport("wininet.dll", SetLastError = true)]
        public static extern IntPtr FindFirstUrlCacheEntry(string lpszUrlSearchPattern, IntPtr lpFirstCacheEntryInfo, out uint lpdwFirstCacheEntryInfoBufferSize);

        [DllImport("wininet.dll", SetLastError = true)]
        public static extern long FindNextUrlCacheEntry(IntPtr hEnumHandle, IntPtr lpNextCacheEntryInfo, out uint lpdwNextCacheEntryInfoBufferSize);

        [DllImport("wininet.dll", SetLastError = true)]
        public static extern bool GetUrlCacheEntryInfo(string lpszUrlName, IntPtr lpCacheEntryInfo, out uint lpdwCacheEntryInfoBufferSize);

        [DllImport("wininet.dll", SetLastError = true)]
        public static extern long DeleteUrlCacheEntry(string lpszUrlName);

        [DllImport("wininet.dll", SetLastError = true)]
        public static extern IntPtr RetrieveUrlCacheEntryStream(string lpszUrlName, IntPtr lpCacheEntryInfo, out uint lpdwCacheEntryInfoBufferSize, long fRandomRead,
            uint dwReserved);

        [DllImport("wininet.dll", SetLastError = true)]
        public static extern IntPtr ReadUrlCacheEntryStream(IntPtr hUrlCacheStream, uint dwLocation, IntPtr lpBuffer, out uint lpdwLen, uint dwReserved);

        [DllImport("wininet.dll", SetLastError = true)]
        public static extern long UnlockUrlCacheEntryStream(IntPtr hUrlCacheStream, uint dwReserved);
    }
}