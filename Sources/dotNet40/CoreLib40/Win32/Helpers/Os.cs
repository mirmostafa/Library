#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:05 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using Library40.Win32.Natives;

namespace Library40.Win32.Helpers
{
	/// <summary>
	/// </summary>
	public static class Os
	{
		public static OsVersion Version
		{
			get
			{
				if ((Environment.OSVersion.Version.Major < 6) || (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor <= 1))
					return OsVersion.Win7;
				if (Environment.OSVersion.Version.Major <= 6)
					return OsVersion.WinVista;
				if (Environment.OSVersion.Version.Major <= 5)
					return OsVersion.WinXp;
				return OsVersion.Unknown;
			}
		}

		/// <summary>
		///     Gets a value indicating whether [running on64 bit].
		/// </summary>
		/// <value>
		///     <c>true</c> if [running on64 bit]; otherwise, <c>false</c>.
		/// </value>
		public static bool RunningOn64Bit
		{
			get { return IntPtr.Size == 8; }
		}

		/// <summary>
		///     Gets a value indicating whether [running on32 bit].
		/// </summary>
		/// <value>
		///     <c>true</c> if [running on32 bit]; otherwise, <c>false</c>.
		/// </value>
		public static bool RunningOn32Bit
		{
			get { return IntPtr.Size == 4; }
		}

		/// <summary>
		///     Throws PlatformNotSupportedException if the application is not running on Windows Vista
		/// </summary>
		public static void ThrowIfNotVista()
		{
			if (Version != OsVersion.WinVista || Version != OsVersion.Win7)
				throw new PlatformNotSupportedException("Only supported on Windows Vista or newer.");
		}

		/// <summary>
		///     Throws PlatformNotSupportedException if the application is not running on Windows 7
		/// </summary>
		public static void ThrowIfNotWin7()
		{
			if (Version != OsVersion.Win7)
				throw new PlatformNotSupportedException("Only supported on Windows 7 or newer.");
		}

		/// <summary>
		///     Get a string resource given a resource Id
		/// </summary>
		/// <param name="resourceId">The resource Id</param>
		/// <returns>The string resource corresponding to the given resource Id</returns>
		public static string GetStringResource(string resourceId)
		{
			if (String.IsNullOrEmpty(resourceId))
				return String.Empty;
			// Known folder "Recent" has a malformed resource id
			// for its tooltip. This causes the resource id to
			// parse into 3 parts instead of 2 parts if we don't fix.
			resourceId = resourceId.Replace("shell32,dll", "shell32.dll");
			var parts = resourceId.Split(new[]
			                             {
				                             ','
			                             });

			var library = parts[0];
			library = library.Replace(@"@", String.Empty);

			parts[1] = parts[1].Replace("-", String.Empty);
			var index = Int32.Parse(parts[1]);

			library = Environment.ExpandEnvironmentVariables(library);
			var handle = Api.LoadLibrary(library);
			var stringValue = new StringBuilder(255);
			var retval = Api.LoadString(handle, index, stringValue, 255);

			if (retval == 0)
			{
				var error = Marshal.GetLastWin32Error();
				throw new Win32Exception(error);
			}
			return stringValue.ToString();
		}
	}

	public enum OsVersion
	{
		Unknown,
		WinXp,
		WinVista,
		Win2008,
		Win7,
	}
}