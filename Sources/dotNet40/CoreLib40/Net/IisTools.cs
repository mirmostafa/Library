#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.IO;
using System.ServiceProcess;
using System.Text;
using Library40.Helpers;

namespace Library40.Net
{
	public class IisTools
	{
		#region IisResetOperation enum
		public enum IisResetOperation
		{
			Stop,
			Start,
			Restart
		}
		#endregion

		private static void ExecCScriptCommandLine(string script, StringBuilder args = null, bool waitForExit = false)
		{
			if (args == null)
				args = new StringBuilder();
			args.Insert(0, script + " ");
			args.Append(" //nologo");
			var str = GetSystem32Dir();
			var startInfo = new ProcessStartInfo();
			if (Debugger.IsAttached)
			{
				args.Insert(0, "CScript.exe ");
				startInfo.FileName = "cmd";
				args.Insert(0, " /K ");
			}
			else
				startInfo.FileName = "CScript.exe";
			startInfo.Arguments = args.ToString();
			startInfo.WorkingDirectory = str;
			startInfo.UseShellExecute = false;
			var process = Process.Start(startInfo);
			if (waitForExit)
				process.WaitForExit();
		}

		private static string GetSystem32Dir()
		{
			return string.Format(@"{0}\System32", Environment.GetEnvironmentVariable("WINDIR"));
		}

		public static void IisReset(bool useService = true, IisResetOperation operation = IisResetOperation.Restart)
		{
			if (useService)
				using (var scm = new ServiceController("IISAdmin"))
				{
					Action stopIis = delegate
					                 {
						                 if (scm.Status == ServiceControllerStatus.Stopped)
							                 return;
						                 scm.Stop();
						                 scm.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromMinutes(1.0));
					                 };
					Action startIis = delegate
					                  {
						                  if (scm.Status == ServiceControllerStatus.Running)
							                  return;
						                  scm.Start();
						                  scm.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromMinutes(1.0));
					                  };
					switch (operation)
					{
						case IisResetOperation.Stop:
							stopIis();
							return;

						case IisResetOperation.Start:
							startIis();
							return;

						case IisResetOperation.Restart:
							stopIis();
							startIis();
							return;
					}
					return;
				}
			switch (operation)
			{
				case IisResetOperation.Stop:
				{
					FileSystemHelper.Start("iisreset", "/stop", waitForExit: true);
					return;
				}
				case IisResetOperation.Start:
				{
					FileSystemHelper.Start("iisreset", "/start", waitForExit: true);
					return;
				}
				case IisResetOperation.Restart:
				{
					FileSystemHelper.Start("iisreset", "/restart", waitForExit: true);
					return;
				}
				default:
					return;
			}
		}

		public static void IisWeb(IEnumerable<string> webSites, bool start = true, string computer = null, string username = null, string password = null)
		{
			var args = new StringBuilder();
			foreach (var str in webSites)
				args.Append(" " + str);
			if (!start)
				args.Append(" /stop");
			if (!computer.IsNullOrEmpty())
				args.AppendFormat(" /s \"{0}\"", computer);
			if (!username.IsNullOrEmpty())
				args.AppendFormat(" /u \"{0}\"", username);
			if (!password.IsNullOrEmpty())
				args.AppendFormat(" /p \"{0}\"", password);
			ExecCScriptCommandLine(@"%SystemRoot%\System32\IIsWeb.vbs", args, true);
		}

		public static bool IisWebSupport()
		{
			return File.Exists(string.Format(@"{0}\IIsWeb.vbs", GetSystem32Dir()));
		}

		public static void RegisterExtentions(string targetSite, string targetVDir, string extention)
		{
			var windowsRoot = Environment.GetEnvironmentVariable("Windir");
			var executablePath = string.Concat(windowsRoot, @"\Microsoft.NET\Framework\v2.0.50727\aspnet_isapi.dll");
			var sitePath = string.Format("IIS://localhost/{0}/Root/{1}", targetSite, targetVDir);
			var iisEntry = new DirectoryEntry(sitePath);
			var applicationMappings = iisEntry.Properties["ScriptMaps"];
			applicationMappings.Add(string.Format(".{0},{1},1,", extention, executablePath));
			iisEntry.CommitChanges();
		}
	}
}