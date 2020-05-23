#region File Notice
// Created at: 2013/12/24 3:46 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.IO;
using System.Linq;
using Library35.LogSystem.FileBased.Xml;

namespace TestConsole35
{
	internal partial class Program
	{
		protected override void Execute()
		{
			var logger = new XmlLogger(new DirectoryInfo(Environment.CurrentDirectory), false);
		}
	}
}