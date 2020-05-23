#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:00 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.IO;
using Library35.LogSystem.Entities;

namespace Library35.LogSystem.FileBased.Xml
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