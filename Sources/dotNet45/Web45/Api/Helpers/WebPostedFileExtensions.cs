#region Code Identifications

// Created on     2018/01/20
// Last update on 2018/01/20 by Mohammad Mir mostafa 

#endregion

using System.Collections.Generic;
using System.Linq;
using Mohammad.Helpers;
using Mohammad.Web.Api.Tools;

namespace Mohammad.Web.Api.Helpers
{
    public static class WebPostedFileExtensions
    {
        public static IEnumerable<WebPostedFile> ByContentType(this IEnumerable<WebPostedFile> file, params string[] contentTypes)
        {
            return file.Where(f => f.ContentType.IsInRange(contentTypes));
        }

        public static IEnumerable<WebPostedFile> ByKeys(this IEnumerable<WebPostedFile> file, params string[] keys)
        {
            return file.Where(f => f.Key.IsInRange(keys));
        }

        public static WebPostedFile ByKey(this IEnumerable<WebPostedFile> file, string key)
        {
            return file.FirstOrDefault(f => f.Key == key);
        }

        public static IEnumerable<string> SaveIn(this IEnumerable<WebPostedFile> files, string absolutePath)
        {
            return files.ForEachFunc(f => f.SaveIn(absolutePath));
        }
    }
}