// Created on     2018/07/23


using System;
using Mohammad.Helpers;
using Mohammad.Win32.Natives.IfacesEnumsStructsClasses;

namespace Mohammad.Net.DataStructs
{
    public class InternetCacheEntryInfo
    {
        public string SourceUrlName { get; }
        public string LocalFileName { get; }
        public uint CacheEntryType { get; }
        public uint UseCount { get; }
        public uint HitRate { get; }
        public DateTime LastModifiedTime { get; }
        public DateTime ExpireTime { get; }
        public DateTime LastAccessTime { get; }
        public DateTime LastSyncTime { get; }
        public IntPtr HeaderInfo { get; }
        public string FileExtension { get; }
        public uint ExemptDelta { get; }

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
            this.SourceUrlName = entry.lpszSourceUrlName.Substring(entry.lpszSourceUrlName.IndexOf("@", StringComparison.Ordinal) + 1);
            this.UseCount = entry.dwUseCount;
        }

        public override string ToString() => this.SourceUrlName;
    }

    public enum UrlCacheType
    {
        Cookie,
        Visited
    }
}