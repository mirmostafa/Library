using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Mohammad.Net.DataStructs;
using Mohammad.Win32.Natives;
using Mohammad.Win32.Natives.IfacesEnumsStructsClasses;

namespace Mohammad.Net
{
    public sealed class InternetCache
    {
        /// <summary>
        ///     UrlCache functionality is taken from:
        ///     Scott McMaster (smcmaste@hotmail.com)
        ///     CodeProject article
        ///     There were some issues with preparing URLs
        ///     for RegExp to work properly. This is
        ///     demonstrated in AllForms.SetupCookieCachePattern method
        ///     urlPattern:
        ///     . Dump the entire contents of the cache.
        ///     Cookie: Lists all cookies on the system.
        ///     Visited: Lists all of the history items.
        ///     Cookie:.*\.example\.com Lists cookies from the example.com domain.
        ///     http://www.example.com/example.html$: Lists the specific named file if present
        ///     \.example\.com: Lists any and all entries from *.example.com.
        ///     \.example\.com.*\.gif$: Lists the .gif files from *.example.com.
        ///     \.js$: Lists the .js files in the cache.
        /// </summary>
        /// <param name="urlPattern"></param>
        /// <returns></returns>
        private static IEnumerable<INTERNET_CACHE_ENTRY_INFO> FindUrlCacheEntries(string urlPattern)
        {
            var results = new List<INTERNET_CACHE_ENTRY_INFO>();

            var buffer = IntPtr.Zero;
            uint structSize;

            //This call will fail but returns the size required in structSize
            //to allocate necessary buffer
            var hEnum = Api.FindFirstUrlCacheEntry(null, buffer, out structSize);
            try
            {
                if (hEnum == IntPtr.Zero)
                {
                    var lastError = Marshal.GetLastWin32Error();
                    switch (lastError)
                    {
                        case Hresults.ERROR_INSUFFICIENT_BUFFER:
                            buffer = Marshal.AllocHGlobal((int) structSize);
                            hEnum = Api.FindFirstUrlCacheEntry(urlPattern, buffer, out structSize);
                            break;
                        case Hresults.ERROR_NO_TOKEN:
                        case Hresults.ERROR_NO_MORE_ITEMS:
                            return results.AsEnumerable();
                    }
                }

                var result = (INTERNET_CACHE_ENTRY_INFO) Marshal.PtrToStructure(buffer, typeof(INTERNET_CACHE_ENTRY_INFO));
                try
                {
                    if (Regex.IsMatch(result.lpszSourceUrlName, urlPattern, RegexOptions.IgnoreCase))
                        results.Add(result);
                }
                catch (ArgumentException ae)
                {
                    throw new ApplicationException("Invalid regular expression, details=" + ae.Message);
                }

                if (buffer != IntPtr.Zero)
                {
                    try
                    {
                        Marshal.FreeHGlobal(buffer);
                    }
                    catch
                    {
                        // ignored
                    }
                    buffer = IntPtr.Zero;
                }

                while (true)
                {
                    var nextResult = Api.FindNextUrlCacheEntry(hEnum, buffer, out structSize);
                    structSize *= 4;
                    if (nextResult != 1) //TRUE
                    {
                        var lastError = Marshal.GetLastWin32Error();
                        switch (lastError)
                        {
                            case Hresults.ERROR_INSUFFICIENT_BUFFER:
                                buffer = Marshal.AllocHGlobal((int) structSize);
                                Api.FindNextUrlCacheEntry(hEnum, buffer, out structSize);
                                break;
                            case Hresults.ERROR_NO_MORE_ITEMS:
                            case Hresults.ERROR_INVALID_PARAMETER:
                                return results.AsEnumerable();
                        }
                    }

                    if (buffer == IntPtr.Zero)
                        continue;
                    result = (INTERNET_CACHE_ENTRY_INFO) Marshal.PtrToStructure(buffer, typeof(INTERNET_CACHE_ENTRY_INFO));
                    if (Regex.IsMatch(result.lpszSourceUrlName, urlPattern, RegexOptions.IgnoreCase))
                        results.Add(result);

                    try
                    {
                        Marshal.FreeHGlobal(buffer);
                    }
                    catch
                    {
                        // ignored
                    }
                    buffer = IntPtr.Zero;
                }
            }
            finally
            {
                if (hEnum != IntPtr.Zero)
                    Api.FindCloseUrlCache(hEnum);
                if (buffer != IntPtr.Zero)
                    try
                    {
                        Marshal.FreeHGlobal(buffer);
                    }
                    catch
                    {
                        // ignored
                    }
            }
        }

        public static IEnumerable<InternetCacheEntryInfo> FindUrlCacheEntries(UrlCacheType type)
            => FindUrlCacheEntries(type + ":").Select(entry => new InternetCacheEntryInfo(entry));

        public static IEnumerable<InternetCacheEntryInfo> FindUrlCacheEntries() { return FindUrlCacheEntries(UrlCacheType.Visited); }
    }
}