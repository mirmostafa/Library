using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;
using Mohammad.Win32.Natives.IfacesEnumsStructsClasses;

namespace Mohammad.Win32.IO
{
    /// <summary>
    ///     Contains information about a file returned by the
    ///     <see cref="FastDirectoryEnumerator" /> class.
    /// </summary>
    [Serializable]
    public class FileData
    {
        /// <summary>
        ///     Attributes of the file.
        /// </summary>
        public readonly FileAttributes Attributes;

        /// <summary>
        ///     File creation time in UTC
        /// </summary>
        public readonly DateTime CreationTimeUtc;

        /// <summary>
        ///     File last access time in UTC
        /// </summary>
        public readonly DateTime LastAccessTimeUtc;

        /// <summary>
        ///     File last write time in UTC
        /// </summary>
        public readonly DateTime LastWriteTimeUtc;

        /// <summary>
        ///     Name of the file
        /// </summary>
        public readonly string Name;

        /// <summary>
        ///     Full path to the file.
        /// </summary>
        public readonly string Path;

        /// <summary>
        ///     Size of the file in bytes
        /// </summary>
        public readonly long Size;

        public DateTime CreationTime { get { return this.CreationTimeUtc.ToLocalTime(); } }

        /// <summary>
        ///     Gets the last access time in local time.
        /// </summary>
        public DateTime LastAccesTime { get { return this.LastAccessTimeUtc.ToLocalTime(); } }

        /// <summary>
        ///     Gets the last access time in local time.
        /// </summary>
        public DateTime LastWriteTime { get { return this.LastWriteTimeUtc.ToLocalTime(); } }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FileData" /> class.
        /// </summary>
        /// <param name="dir">The directory that the file is stored at</param>
        /// <param name="findData">
        ///     WIN32_FIND_DATA structure that this
        ///     object wraps.
        /// </param>
        internal FileData(string dir, WIN32_FIND_DATA findData)
        {
            this.Attributes = findData.dwFileAttributes;

            this.CreationTimeUtc = ConvertDateTime(findData.ftCreationTime_dwHighDateTime, findData.ftCreationTime_dwLowDateTime);

            this.LastAccessTimeUtc = ConvertDateTime(findData.ftLastAccessTime_dwHighDateTime, findData.ftLastAccessTime_dwLowDateTime);

            this.LastWriteTimeUtc = ConvertDateTime(findData.ftLastWriteTime_dwHighDateTime, findData.ftLastWriteTime_dwLowDateTime);

            this.Size = CombineHighLowInts(findData.nFileSizeHigh, findData.nFileSizeLow);

            this.Name = findData.cFileName;
            this.Path = System.IO.Path.Combine(dir, findData.cFileName);
        }

        private static long CombineHighLowInts(uint high, uint low) { return ((long) high << 0x20) | low; }

        private static DateTime ConvertDateTime(uint high, uint low)
        {
            var fileTime = CombineHighLowInts(high, low);
            return DateTime.FromFileTimeUtc(fileTime);
        }

        public override string ToString() { return this.Name; }
    }

    /// <summary>
    ///     A fast enumerator of files in a directory.  Use this if you need to get attributes for
    ///     all files in a directory.
    /// </summary>
    /// <remarks>
    ///     This enumerator is substantially faster than using <see cref="Directory.GetFiles(string)" />
    ///     and then creating a new FileInfo object for each path.  Use this version when you
    ///     will need to look at the attibutes of each file returned (for example, you need
    ///     to check each file in a directory to see if it was modified after a specific date).
    /// </remarks>
    public static class FastDirectoryEnumerator
    {
        /// <summary>
        ///     Gets <see cref="FileData" /> for all the files in a directory.
        /// </summary>
        /// <param name="path">The path to search.</param>
        /// <returns>
        ///     An object that implements <see cref="IEnumerable{T}" /> and
        ///     allows you to enumerate the files in the given directory.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="path" /> is a null reference (Nothing in VB)
        /// </exception>
        public static IEnumerable<FileData> EnumerateFiles(string path)
        {
            return EnumerateFiles(path, "*");
        }

        /// <summary>
        ///     Gets <see cref="FileData" /> for all the files in a directory that match a
        ///     specific filter.
        /// </summary>
        /// <param name="path">The path to search.</param>
        /// <param name="searchPattern">The search string to match against files in the path.</param>
        /// <returns>
        ///     An object that implements <see cref="IEnumerable{FileData}" /> and
        ///     allows you to enumerate the files in the given directory.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="path" /> is a null reference (Nothing in VB)
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="searchPattern" /> is a null reference (Nothing in VB)
        /// </exception>
        public static IEnumerable<FileData> EnumerateFiles(string path, string searchPattern)
        {
            return EnumerateFiles(path, searchPattern, SearchOption.TopDirectoryOnly);
        }

        /// <summary>
        ///     Gets <see cref="FileData" /> for all the files in a directory that
        ///     match a specific filter, optionally including all sub directories.
        /// </summary>
        /// <param name="path">The path to search.</param>
        /// <param name="searchPattern">The search string to match against files in the path.</param>
        /// <param name="searchOption">
        ///     One of the SearchOption values that specifies whether the search
        ///     operation should include all subdirectories or only the current directory.
        /// </param>
        /// <returns>
        ///     An object that implements <see cref="IEnumerable{FileData}" /> and
        ///     allows you to enumerate the files in the given directory.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="path" /> is a null reference (Nothing in VB)
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="searchPattern" /> is a null reference (Nothing in VB)
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="searchOption" /> is not one of the valid values of the
        ///     <see cref="System.IO.SearchOption" /> enumeration.
        /// </exception>
        public static IEnumerable<FileData> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            if (searchPattern == null)
                throw new ArgumentNullException("searchPattern");
            if (searchOption != SearchOption.TopDirectoryOnly && searchOption != SearchOption.AllDirectories)
                throw new ArgumentOutOfRangeException("searchOption");

            var fullPath = Path.GetFullPath(path);

            return new FileEnumerable(fullPath, searchPattern, searchOption);
        }

        /// <summary>
        ///     Gets <see cref="FileData" /> for all the files in a directory that match a
        ///     specific filter.
        /// </summary>
        /// <param name="path">The path to search.</param>
        /// <param name="searchPattern">The search string to match against files in the path.</param>
        /// <param name="searchOption"></param>
        /// <returns>
        ///     An object that implements <see cref="IEnumerable{FileData}" /> and
        ///     allows you to enumerate the files in the given directory.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="path" /> is a null reference (Nothing in VB)
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="searchPattern" /> is a null reference (Nothing in VB)
        /// </exception>
        public static FileData[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            var e = EnumerateFiles(path, searchPattern, SearchOption.TopDirectoryOnly);
            var list = new List<FileData>(e);

            var retval = new FileData[list.Count];
            list.CopyTo(retval);

            return retval;
        }

        /// <summary>
        ///     Provides the implementation of the
        ///     <see cref="T:System.Collections.Generic.IEnumerable`1" /> interface
        /// </summary>
        private class FileEnumerable : IEnumerable<FileData>
        {
            private readonly string _Filter;
            private readonly string _Path;
            private readonly SearchOption _SearchOption;

            /// <summary>
            ///     Initializes a new instance of the <see cref="FileEnumerable" /> class.
            /// </summary>
            /// <param name="path">The path to search.</param>
            /// <param name="filter">The search string to match against files in the path.</param>
            /// <param name="searchOption">
            ///     One of the SearchOption values that specifies whether the search
            ///     operation should include all subdirectories or only the current directory.
            /// </param>
            public FileEnumerable(string path, string filter, SearchOption searchOption)
            {
                this._Path = path;
                this._Filter = filter;
                this._SearchOption = searchOption;
            }

            /// <summary>
            ///     Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            ///     A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can
            ///     be used to iterate through the collection.
            /// </returns>
            public IEnumerator<FileData> GetEnumerator()
            {
                return new FileEnumerator(this._Path, this._Filter, this._SearchOption);
            }

            /// <summary>
            ///     Returns an enumerator that iterates through a collection.
            /// </summary>
            /// <returns>
            ///     An <see cref="T:System.Collections.IEnumerator" /> object that can be
            ///     used to iterate through the collection.
            /// </returns>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return new FileEnumerator(this._Path, this._Filter, this._SearchOption);
            }
        }

        /// <summary>
        ///     Provides the implementation of the
        ///     <see cref="T:System.Collections.Generic.IEnumerator`1" /> interface
        /// </summary>
        [SuppressUnmanagedCodeSecurity]
        private class FileEnumerator : IEnumerator<FileData>
        {
            private readonly string _Filter;
            private readonly Stack<SearchContext> _Handles;
            private readonly SearchOption _SearchOption;
            private readonly WIN32_FIND_DATA _Win32FindData = new WIN32_FIND_DATA();
            private SafeFindHandle _FindFileHandle;
            private string _Path;

            /// <summary>
            ///     Initializes a new instance of the <see cref="FileEnumerator" /> class.
            /// </summary>
            /// <param name="path">The path to search.</param>
            /// <param name="filter">The search string to match against files in the path.</param>
            /// <param name="searchOption">
            ///     One of the SearchOption values that specifies whether the search
            ///     operation should include all subdirectories or only the current directory.
            /// </param>
            public FileEnumerator(string path, string filter, SearchOption searchOption)
            {
                this._Path = path;
                this._Filter = filter;
                this._SearchOption = searchOption;

                if (this._SearchOption == SearchOption.AllDirectories)
                    this._Handles = new Stack<SearchContext>();
            }

            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern SafeFindHandle FindFirstFile(string fileName, [In] [Out] WIN32_FIND_DATA data);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern bool FindNextFile(SafeFindHandle hndFindFile, [In] [Out] [MarshalAs(UnmanagedType.LPStruct)] WIN32_FIND_DATA lpFindFileData);

            /// <summary>
            ///     Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            /// <value></value>
            /// <returns>
            ///     The element in the collection at the current position of the enumerator.
            /// </returns>
            public FileData Current { get { return new FileData(this._Path, this._Win32FindData); } }

            /// <summary>
            ///     Performs application-defined tasks associated with freeing, releasing,
            ///     or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                if (!this._FindFileHandle.IsClosed)
                    this._FindFileHandle.Dispose();
            }

            /// <summary>
            ///     Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            /// <value></value>
            /// <returns>
            ///     The element in the collection at the current position of the enumerator.
            /// </returns>
            object IEnumerator.Current { get { return new FileData(this._Path, this._Win32FindData); } }

            /// <summary>
            ///     Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns>
            ///     true if the enumerator was successfully advanced to the next element;
            ///     false if the enumerator has passed the end of the collection.
            /// </returns>
            /// <exception cref="T:System.InvalidOperationException">
            ///     The collection was modified after the enumerator was created.
            /// </exception>
            public bool MoveNext()
            {
                bool retval;

                //If the handle is null, this is first call to MoveNext in the current 
                // directory.  In that case, start a new search.
                if (this._FindFileHandle == null)
                {
                    new FileIOPermission(FileIOPermissionAccess.PathDiscovery, this._Path).Demand();

                    var searchPath = Path.Combine(this._Path, this._Filter);
                    this._FindFileHandle = FindFirstFile(searchPath, this._Win32FindData);
                    retval = !this._FindFileHandle.IsInvalid;
                }
                else
                    //Otherwise, find the next item.
                {
                    retval = FindNextFile(this._FindFileHandle, this._Win32FindData);
                }

                //If the call to FindNextFile or FindFirstFile succeeded.
                if (retval)
                {
                    if (this._Win32FindData.cFileName == "." || this._Win32FindData.cFileName == "..") //Ignore the special "." and ".." folders.   We 
                        //call MoveNext recursively here to move to the next item 
                        //that FindNextFile will return.
                        return this.MoveNext();

                    //If we are now on a directory...
                    if (this._Win32FindData.dwFileAttributes == FileAttributes.Directory)
                    {
                        //If we are searching directories, push the current directory onto the 
                        // context stack, then restart the search in the sub-directory. 
                        //Otherwise, skip the directory and move on to the next file in the current 
                        // directory.
                        if (this._SearchOption == SearchOption.AllDirectories)
                        {
                            var context = new SearchContext(this._FindFileHandle, this._Path);
                            this._Handles.Push(context);

                            this._Path = Path.Combine(this._Path, this._Win32FindData.cFileName);
                            this._FindFileHandle = null;
                        }

                        return this.MoveNext();
                    }
                }
                else if (this._SearchOption == SearchOption.AllDirectories) //If there are no more files in this directory and we are 
                    // in a sub directory, pop back up to the parent directory and
                    // continue the search from there.
                {
                    if (this._Handles.Count > 0)
                    {
                        var context = this._Handles.Pop();
                        this._Path = context.Path;
                        this._FindFileHandle = context.Handle;

                        return this.MoveNext();
                    }
                }

                return retval;
            }

            /// <summary>
            ///     Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            /// <exception cref="T:System.InvalidOperationException">
            ///     The collection was modified after the enumerator was created.
            /// </exception>
            public void Reset()
            {
                this._FindFileHandle = null;
            }

            /// <summary>
            ///     Hold context information about where we current are in the directory search.
            /// </summary>
            private class SearchContext
            {
                public readonly SafeFindHandle Handle;
                public readonly string Path;

                public SearchContext(SafeFindHandle handle, string path)
                {
                    this.Handle = handle;
                    this.Path = path;
                }
            }
        }

        /// <summary>
        ///     Wraps a FindFirstFile handle.
        /// </summary>
        private sealed class SafeFindHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
            /// <summary>
            ///     Initializes a new instance of the <see cref="SafeFindHandle" /> class.
            /// </summary>
            [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
            internal SafeFindHandle()
                : base(true) {}

            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
            [DllImport("kernel32.dll")]
            private static extern bool FindClose(IntPtr handle);

            /// <summary>
            ///     When overridden in a derived class, executes the code required to free the handle.
            /// </summary>
            /// <returns>
            ///     true if the handle is released successfully; otherwise, in the
            ///     event of a catastrophic failure, false. In this case, it
            ///     generates a releaseHandleFailed MDA Managed Debugging Assistant.
            /// </returns>
            protected override bool ReleaseHandle()
            {
                return FindClose(this.handle);
            }
        }
    }
}