#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using Library40.EventsArgs;
using Library40.Win32.Natives;
using Library40.Win32.Natives.IfacesEnumsStructsClasses;
using Microsoft.VisualBasic.FileIO;

namespace Library40.Helpers
{
	/// <summary>
	/// </summary>
	public static class FileSystemHelper
	{
		/// <summary>
		///     Gets the image files.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns></returns>
		public static IEnumerable<FileInfo> GetImageFiles(this DirectoryInfo path)
		{
			var files = new Collection<FileInfo>();
			"*.bmp,*.jpeg,*.jpg,*.png".Split(',').ForEach(extension => path.GetFiles(extension).ForEach(files.Add));
			return files.AsEnumerable();
		}

		/// <summary>
		///     Executes the specified file or directory.
		/// </summary>
		/// <param name="fileSystem">The file system.</param>
		public static void ShellExecute(this FileSystemInfo fileSystem)
		{
			Api.ShellExecute(IntPtr.Zero, ShellExecuteVerbs.OpenFile, fileSystem.FullName, null, null, ShowCommands.SW_SHOWNORMAL);
		}

		/// <summary>
		///     Gets the count of Files in source.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns></returns>
		public static int GetFileSystemCount(this DirectoryInfo source)
		{
			var num = source.GetFiles().Count();
			foreach (var info in source.GetDirectories())
			{
				num += info.GetFileSystemCount();
				num++;
			}
			return num;
		}

		/// <summary>
		///     Safely deletes the specific files.
		/// </summary>
		/// <param name="files">The files.</param>
		/// <param name="onFileDeleting">The on file deleting.</param>
		/// <param name="sendToRecycleBin">
		///     if set to <c>true</c> sends to recycle bin.
		/// </param>
		public static void SafeDelete(this IEnumerable<FileInfo> files, EventHandler<ItemActingEventArgs<FileInfo>> onFileDeleting = null, bool sendToRecycleBin = false)
		{
			files.ForEach(file => file.SafeDelete(onFileDeleting, sendToRecycleBin));
		}

		/// <summary>
		///     Safely deletes the specific file.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="onFileDeleting">The on file deleting.</param>
		/// <param name="sendToRecycleBin">
		///     if set to <c>true</c> sends to recycle bin.
		/// </param>
		public static void SafeDelete(this FileInfo file, EventHandler<ItemActingEventArgs<FileInfo>> onFileDeleting = null, bool sendToRecycleBin = false)
		{
			if (onFileDeleting != null)
			{
				var e = new ItemActingEventArgs<FileInfo>(file);
				onFileDeleting(null, e);
				if (e.Handled)
					return;
			}
			file.Attributes = FileAttributes.Normal;
			FileSystem.DeleteFile(file.FullName, UIOption.OnlyErrorDialogs, sendToRecycleBin ? RecycleOption.SendToRecycleBin : RecycleOption.DeletePermanently);
		}

		/// <summary>
		///     Safely deletes the specific directories.
		/// </summary>
		/// <param name="directories">The directories.</param>
		/// <param name="onDirectoryDeleting">The on directory deleting.</param>
		/// <param name="onFileDeleting">The on file deleting.</param>
		/// <param name="sendToRecycleBin">
		///     if set to <c>true</c> sends to recycle bin.
		/// </param>
		public static void SafeDelete(this IEnumerable<DirectoryInfo> directories,
			EventHandler<ItemActingEventArgs<DirectoryInfo>> onDirectoryDeleting = null,
			EventHandler<ItemActingEventArgs<FileInfo>> onFileDeleting = null,
			bool sendToRecycleBin = false)
		{
			directories.ForEach(dir => dir.SafeDelete(onDirectoryDeleting, onFileDeleting, sendToRecycleBin));
		}

		/// <summary>
		///     Safely deletes the specific file system.
		/// </summary>
		/// <param name="fileSystem">The file system.</param>
		/// <param name="onFileSystemDeleting">The on file system deleting.</param>
		/// <param name="onFileDeleting">The on file deleting.</param>
		/// <param name="sendToRecycleBin">
		///     if set to <c>true</c> sends to recycle bin.
		/// </param>
		public static void SafeDelete(this FileSystemInfo fileSystem,
			EventHandler<ItemActingEventArgs<FileSystemInfo>> onFileSystemDeleting = null,
			EventHandler<ItemActingEventArgs<FileInfo>> onFileDeleting = null,
			bool sendToRecycleBin = false)
		{
			if (IsDirectory(fileSystem.FullName))
			{
				var onDirectoryDeleting = new EventHandler<ItemActingEventArgs<DirectoryInfo>>(delegate(object sender, ItemActingEventArgs<DirectoryInfo> e)
				                                                                               {
					                                                                               if (e == null)
						                                                                               return;
					                                                                               var args = new ItemActingEventArgs<FileSystemInfo>(e.Item);
					                                                                               if (onFileSystemDeleting != null)
						                                                                               onFileSystemDeleting(sender, args);
					                                                                               e.Handled = args.Handled;
				                                                                               });
				fileSystem.As<DirectoryInfo>().SafeDelete(onDirectoryDeleting, onFileDeleting, sendToRecycleBin);
			}
			else
			{
				var handler = new EventHandler<ItemActingEventArgs<FileInfo>>(delegate(object sender, ItemActingEventArgs<FileInfo> e)
				                                                              {
					                                                              if (e == null)
						                                                              return;
					                                                              var args = new ItemActingEventArgs<FileSystemInfo>(e.Item);
					                                                              if (onFileSystemDeleting != null)
						                                                              onFileSystemDeleting(sender, args);
					                                                              e.Handled = args.Handled;
				                                                              });
				fileSystem.As<FileInfo>().SafeDelete(handler, sendToRecycleBin);
			}
		}

		/// <summary>
		///     Safely deletes the specific directory.
		/// </summary>
		/// <param name="directory">The directory.</param>
		/// <param name="onDirectoryDeleting">The on directory deleting.</param>
		/// <param name="onFileDeleting">The on file deleting.</param>
		/// <param name="sendToRecycleBin">
		///     if set to <c>true</c> sends to recycle bin.
		/// </param>
		public static void SafeDelete(this DirectoryInfo directory,
			EventHandler<ItemActingEventArgs<DirectoryInfo>> onDirectoryDeleting = null,
			EventHandler<ItemActingEventArgs<FileInfo>> onFileDeleting = null,
			bool sendToRecycleBin = false)
		{
			if (!directory.Exists)
				return;
			foreach (var info in directory.GetFiles())
			{
				if (onFileDeleting != null)
				{
					var e = new ItemActingEventArgs<FileInfo>(info);
					onFileDeleting(null, e);
					if (e.Handled)
						continue;
				}
				info.SafeDelete(onFileDeleting, sendToRecycleBin);
			}
			foreach (var info2 in directory.GetDirectories())
			{
				if (onDirectoryDeleting != null)
				{
					var args2 = new ItemActingEventArgs<DirectoryInfo>(info2);
					onDirectoryDeleting(null, args2);
					if (args2.Handled)
						continue;
				}
				info2.SafeDelete(onDirectoryDeleting, onFileDeleting, sendToRecycleBin);
			}
			try
			{
				Directory.Delete(directory.FullName);
			}
			catch
			{
			}
		}

		/// <summary>
		///     Copies the source files recursively.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="overwrite">
		///     if set to <c>true</c> the file will be overwritten if exists.
		/// </param>
		/// <param name="onCopy">Will be called before copying each file. The file won't be copied if returns false.</param>
		/// <param name="retryOnException"></param>
		public static void Copy(this DirectoryInfo source,
			DirectoryInfo destination,
			bool overwrite = true,
			Predicate<FileSystemInfo> onCopy = null,
			Func<FileInfo, Exception, bool> retryOnException = null)
		{
			foreach (var file in source.GetFiles().Where(file => onCopy == null || onCopy(file)))
				Copy(file, destination, overwrite, retryOnException);
			foreach (var dir in source.GetDirectories())
				if ((onCopy == null) || onCopy(dir))
				{
					var info = new DirectoryInfo(string.Concat(destination.FullName, "\\", dir.Name));
					if (!info.Exists)
						info.Create();
					dir.Copy(info, overwrite, onCopy, retryOnException);
				}
		}

		/// <summary>
		///     Copies the specified file.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="overwrite">
		///     if set to <c>true</c> the file will be overwritten if exists.
		/// </param>
		/// <param name="retryOnException">Will be called before copying each file. The file won't be copied if returns false.</param>
		public static void Copy(this FileInfo file, DirectoryInfo destination, bool overwrite = true, Func<FileInfo, Exception, bool> retryOnException = null)
		{
			try
			{
				file.CopyTo(destination.FullName + @"\" + file.Name, overwrite);
			}
			catch
			{
				var fileSecurity = file.GetAccessControl();
				fileSecurity.AddAccessRule(new FileSystemAccessRule(WindowsIdentity.GetCurrent().Name, FileSystemRights.FullControl, AccessControlType.Allow));
				file.SetAccessControl(fileSecurity);
				file.Attributes &= ~FileAttributes.ReadOnly;

				var directorySecurity = destination.GetAccessControl();
				directorySecurity.AddAccessRule(new FileSystemAccessRule(WindowsIdentity.GetCurrent().Name, FileSystemRights.FullControl, AccessControlType.Allow));
				destination.SetAccessControl(directorySecurity);
				destination.Attributes &= ~FileAttributes.ReadOnly;

				new FileInfo(destination.FullName + @"\" + file.Name).Attributes &= ~FileAttributes.ReadOnly;
				try
				{
					file.CopyTo(destination.FullName + @"\" + file.Name, overwrite);
				}
				catch (Exception ex)
				{
					if (retryOnException != null)
					{
						if (retryOnException(file, ex))
							Copy(file, destination, overwrite, retryOnException);
					}
					else
						throw;
				}
			}
		}

		/// <summary>
		///     Gets all files in specific directory.
		/// </summary>
		/// <param name="directory">The directory.</param>
		/// <returns>Returns all files in specific directory</returns>
		public static IEnumerable<string> GetAllFiles(this DirectoryInfo directory)
		{
			foreach (var fileName in directory.GetFiles().Select(file => file.FullName))
				yield return fileName;
			foreach (var fileName in directory.GetDirectories().SelectMany(dir => dir.GetAllFiles()))
				yield return fileName;
		}

		/// <summary>
		///     Gets all files.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns></returns>
		public static IEnumerable<string> GetAllFiles(string path)
		{
			return new DirectoryInfo(path).GetAllFiles();
		}

		/// <summary>
		///     Gets all directories.
		/// </summary>
		/// <param name="directory">The directory.</param>
		/// <returns></returns>
		public static IEnumerable<string> GetAllDirectories(this DirectoryInfo directory)
		{
			foreach (var fileName in directory.GetDirectories().Select(dir => dir.FullName))
				yield return fileName;
			foreach (var fileName in directory.GetDirectories().SelectMany(dir => dir.GetAllDirectories()))
				yield return fileName;
		}

		/// <summary>
		///     Gets all directories.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns></returns>
		public static IEnumerable<string> GetAllDirectories(string path)
		{
			return new DirectoryInfo(path).GetAllDirectories();
		}

		/// <summary>
		///     Gets all file systems.
		/// </summary>
		/// <param name="directory">The directory.</param>
		/// <returns></returns>
		public static IEnumerable<string> GetAllFileSystems(this DirectoryInfo directory)
		{
			foreach (var dir in directory.GetDirectories())
			{
				yield return dir.FullName;
				foreach (var file in dir.GetFiles())
					yield return file.FullName;
				foreach (var fileSystem in dir.GetAllFileSystems())
					yield return fileSystem;
			}
		}

		/// <summary>
		///     Gets all file systems.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns></returns>
		public static IEnumerable<string> GetAllFileSystems(string path)
		{
			return new DirectoryInfo(path).GetAllFileSystems();
		}

		/// <summary>
		///     Executes the specified command.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <param name="args">The command parameters.</param>
		/// <param name="workingDirectory">The working directory.</param>
		/// <param name="waitForExit">
		///     if set to <c>true</c> [wait for exit].
		/// </param>
		/// <param name="useShellExecute">
		///     if set to <c>true</c> [use shell execute].
		/// </param>
		/// <param name="createNoWindow">
		///     if set to <c>true</c> [create no window].
		/// </param>
		public static void Start(string command,
			string args = null,
			string workingDirectory = null,
			bool waitForExit = false,
			bool useShellExecute = false,
			bool createNoWindow = true)
		{
			if (workingDirectory.IsNullOrEmpty())
				workingDirectory = GetSystem32Dir();
			var startInfo = new ProcessStartInfo
			                {
				                WorkingDirectory = workingDirectory,
				                UseShellExecute = useShellExecute,
				                FileName = command,
				                Arguments = args,
				                CreateNoWindow = createNoWindow
			                };
			var result = Process.Start(startInfo);
			if (waitForExit)
				result.WaitForExit();
		}

		/// <summary>
		///     Gets the system32 dir.
		/// </summary>
		/// <returns></returns>
		public static string GetSystem32Dir()
		{
			return String.Format(@"{0}\System32", Environment.GetEnvironmentVariable("WINDIR"));
		}

		/// <summary>
		///     Determines whether the specified path is file.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns>
		///     <c>true</c> if the specified path is file; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsFile(string path)
		{
			var result = File.Exists(path);
			return result || !string.IsNullOrEmpty(Path.GetExtension(path));
		}

		/// <summary>
		///     Determines whether the specified path is directory.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns>
		///     <c>true</c> if the specified path is directory; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsDirectory(string path)
		{
			var result = Directory.Exists(path);
			return result || string.IsNullOrEmpty(Path.GetExtension(path));
		}

		/// <summary>
		///     Adds a directory security.
		/// </summary>
		/// <param name="folderName">Name of the folder.</param>
		/// <param name="account">The account.</param>
		/// <param name="rights">The rights.</param>
		/// <param name="controlType">Type of the control.</param>
		public static void AddDirectorySecurity(string folderName, string account, FileSystemRights rights, AccessControlType controlType)
		{
			var directoryInfo = new DirectoryInfo(folderName);
			var directorySecurity = directoryInfo.GetAccessControl();
			directorySecurity.AddAccessRule(new FileSystemAccessRule(account,
				rights,
				InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
				PropagationFlags.None,
				controlType));
			directoryInfo.SetAccessControl(directorySecurity);
		}

		/// <summary>
		///     Removes the directory security.
		/// </summary>
		/// <param name="folderName">Name of the folder.</param>
		/// <param name="account">The account.</param>
		/// <param name="rights">The rights.</param>
		/// <param name="controlType">Type of the control.</param>
		public static void RemoveDirectorySecurity(string folderName, string account, FileSystemRights rights, AccessControlType controlType)
		{
			var directoryInfo = new DirectoryInfo(folderName);
			var directorySecurity = directoryInfo.GetAccessControl();
			directorySecurity.RemoveAccessRule(new FileSystemAccessRule(account, rights, controlType));
			directoryInfo.SetAccessControl(directorySecurity);
		}

		/// <summary>
		///     Adds the security.
		/// </summary>
		/// <param name="dir">The dir.</param>
		/// <param name="account">The account.</param>
		/// <param name="rights">The rights.</param>
		/// <param name="controlType">Type of the control.</param>
		public static void AddSecurity(this DirectoryInfo dir, string account, FileSystemRights rights, AccessControlType controlType)
		{
			AddDirectorySecurity(dir.FullName, account, rights, controlType);
		}

		/// <summary>
		///     Removes the security.
		/// </summary>
		/// <param name="dir">The dir.</param>
		/// <param name="account">The account.</param>
		/// <param name="rights">The rights.</param>
		/// <param name="controlType">Type of the control.</param>
		public static void RemoveSecurity(this DirectoryInfo dir, string account, FileSystemRights rights, AccessControlType controlType)
		{
			RemoveDirectorySecurity(dir.FullName, account, rights, controlType);
		}
	}
}