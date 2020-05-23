#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:59 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using Library35.Helpers;
using Library35.Win32.Natives.IfacesEnumsStructsClasses;

namespace Library35.Net.DataStructs
{
	public class InternetCacheEntryInfo
	{
		internal InternetCacheEntryInfo(INTERNET_CACHE_ENTRY_INFO entry)
		{
			this.CacheEntryType = entry.CacheEntryType;
			this.ExemptDelta = entry.dwExemptDelta;
			this.ExpireTime = entry.ExpireTime.ToDateTime();
			this.FileExtension = entry.lpszFileExtension;
			this.HeaderInfo = entry.lpHeaderInfo;
			this.HitRate = entry.dwHitRate;
			this.LastAccessTime = entry.LastAccessTime.ToDateTime();
			this.LastModifiedTime = entry.LastModifiedTime.ToDateTime();
			this.LastSyncTime = entry.LastSyncTime.ToDateTime();
			this.LocalFileName = entry.lpszLocalFileName;
			this.SourceUrlName = entry.lpszSourceUrlName.Substring(entry.lpszSourceUrlName.IndexOf("@") + 1);
			this.UseCount = entry.dwUseCount;
		}

		public string SourceUrlName { get; private set; }

		public string LocalFileName { get; private set; }

		public UInt32 CacheEntryType { get; private set; }

		public UInt32 UseCount { get; private set; }

		public UInt32 HitRate { get; private set; }

		public DateTime LastModifiedTime { get; private set; }

		public DateTime ExpireTime { get; private set; }

		public DateTime LastAccessTime { get; private set; }

		public DateTime LastSyncTime { get; private set; }

		public IntPtr HeaderInfo { get; private set; }

		public string FileExtension { get; private set; }

		public UInt32 ExemptDelta { get; private set; }

		public override string ToString()
		{
			return this.SourceUrlName;
		}
	}

	public enum UrlCacheType
	{
		Cookie,
		Visited,
	}
}