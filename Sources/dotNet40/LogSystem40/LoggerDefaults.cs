#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:06 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.IO;
using Library40.Helpers;

namespace Library40.LogSystem
{
	public class LoggerDefaults
	{
		public static DirectoryInfo DefaultDirectory
		{
			get { return new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), ApplicationHelper.Company, "Logs")); }
		}

		public static string GenerateLogFileSpec()
		{
			return Path.Combine(DefaultDirectory.FullName, string.Concat(ApplicationHelper.ProductTitle, ".log"));
		}
	}
}