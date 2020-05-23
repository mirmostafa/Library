#region Code Identifications

// Created on     2017/07/29
// Last update on 2018/03/12 by Mohammad Mir mostafa 

#endregion

using System;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using Mohammad.Helpers;

namespace Mohammad.Logging
{
    public static class IsolatedStorage
    {
        public static Func<IsolatedStorageFile> NewStore
        {
            get { return () => IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null); }
        }

        public static string CreateDirectory(this IsolatedStorageFile store, string relativePath, bool isNotExists)
        {
            var path = Prepare(relativePath, true);
            store.CreateDirectory(path);
            return path;
        }

        public static string CreateFile(this IsolatedStorageFile store, string relativePath, string fileName, bool isNotExists)
        {
            var path = Path.Combine(Prepare(relativePath, true), Prepare(fileName, true));
            if (isNotExists && store.FileExists(path))
                return path;
            using (var stream = store.CreateFile(path))
                stream.Flush();
            return path;
        }

        public static void WriteLine(this IsolatedStorageFile store, string log, string fileName) { WriteLine(store, log, null, fileName); }

        public static void WriteLine(this IsolatedStorageFile store, string log, string relativePath, string fileName)
        {
            var path = relativePath.IsNullOrEmpty() ? Prepare(fileName, true) : Path.Combine(Prepare(relativePath, true), Prepare(fileName, true));
            using (var writer = new StreamWriter(store.OpenFile(path, FileMode.Append)))
                writer.WriteLine(Prepare(log, false));
        }

        private static string Prepare(string text, bool validatePathChars)
        {
            if (validatePathChars && text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(text));
            if (text.IsNullOrEmpty())
                return text;
            if (text.Contains("%now%"))
            {
                //Path.GetInvalidFileNameChars()
                var now = DateTime.Now;
                string nowS;
                if (validatePathChars)
                {
                    now = now.Subtract(TimeSpan.FromSeconds(now.Second));
                    nowS = now.ToString(CultureInfo.InvariantCulture);
                    nowS = Path.GetInvalidFileNameChars().Aggregate(nowS, (current, c) => current.Replace(c, '-'));
                }
                else
                    nowS = now.ToString(CultureInfo.InvariantCulture);
                text = text.Replace("%now%", nowS);
            }
            return text;
        }
    }
}