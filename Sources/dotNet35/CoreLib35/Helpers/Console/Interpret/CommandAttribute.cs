#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Linq;

namespace Library35.Helpers.Console.Interpret
{
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	internal sealed class CommandAttribute : Attribute
	{
		public string HelpText { get; set; }
	}
}