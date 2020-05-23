#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:06 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.IO;
using Library40.LogSystem.Entities;

namespace Library40.LogSystem.FileBased.Xml
{
	/// <summary>
	///     Facade for writing a log entry(es) optimized for XML log writer. This class cannot be inherited.
	/// </summary>
	public sealed class XmlLogger : XmlLogger<LogEntity>
	{
		public XmlLogger(DirectoryInfo logPath = null, bool useLogRotation = false)
			: base(logPath, useLogRotation)
		{
		}
	}
}