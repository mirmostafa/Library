using System;
using Mohammad.Helpers;
using Mohammad.Win32.Natives.IfacesEnumsStructsClasses;

namespace Mohammad.Net.DataStructs
{
    public class InternetCacheEntryInfo
    {
        public string SourceUrlName { get; }
        public string LocalFileName { get; private set; }
        public uint CacheEntryType { get; private set; }
        public uint UseCount { get; private set; }
        public uint HitRate { get; private set; }
        public DateTime LastModifiedTime { get; private set; }
        public DateTime ExpireTime { get; private set; }
        public DateTime LastAccessTime { get; private set; }
        public DateTime LastSyncTime { get; private set; }
        public IntPtr HeaderInfo { get; private set; }
        public string FileExtension { get; private set; }
        public uint ExemptDelta { get; private set; }

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

        public override string ToString() { return this.SourceUrlName; }
    }

    public enum UrlCacheType
    {
        Cookie,
        Visited
    }
}