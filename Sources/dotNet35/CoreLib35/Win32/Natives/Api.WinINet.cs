#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:59 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Runtime.InteropServices;

namespace Library35.Win32.Natives
{
	partial class Api
	{
		[DllImport("wininet.dll", SetLastError = true)]
		public static extern long FindCloseUrlCache(IntPtr hEnumHandle);

		[DllImport("wininet.dll", SetLastError = true)]
		public static extern IntPtr FindFirstUrlCacheEntry(string lpszUrlSearchPattern, IntPtr lpFirstCacheEntryInfo, out UInt32 lpdwFirstCacheEntryInfoBufferSize);

		[DllImport("wininet.dll", SetLastError = true)]
		public static extern long FindNextUrlCacheEntry(IntPtr hEnumHandle, IntPtr lpNextCacheEntryInfo, out UInt32 lpdwNextCacheEntryInfoBufferSize);

		[DllImport("wininet.dll", SetLastError = true)]
		public static extern bool GetUrlCacheEntryInfo(string lpszUrlName, IntPtr lpCacheEntryInfo, out UInt32 lpdwCacheEntryInfoBufferSize);

		[DllImport("wininet.dll", SetLastError = true)]
		public static extern long DeleteUrlCacheEntry(string lpszUrlName);

		[DllImport("wininet.dll", SetLastError = true)]
		public static extern IntPtr RetrieveUrlCacheEntryStream(string lpszUrlName,
			IntPtr lpCacheEntryInfo,
			out UInt32 lpdwCacheEntryInfoBufferSize,
			long fRandomRead,
			UInt32 dwReserved);

		[DllImport("wininet.dll", SetLastError = true)]
		public static extern IntPtr ReadUrlCacheEntryStream(IntPtr hUrlCacheStream, UInt32 dwLocation, IntPtr lpBuffer, out UInt32 lpdwLen, UInt32 dwReserved);

		[DllImport("wininet.dll", SetLastError = true)]
		public static extern long UnlockUrlCacheEntryStream(IntPtr hUrlCacheStream, UInt32 dwReserved);
	}
}