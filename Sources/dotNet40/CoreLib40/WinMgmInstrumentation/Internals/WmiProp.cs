#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:05 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;

namespace Library40.WinMgmInstrumentation.Internals
{
	[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public sealed class WmiPropAttribute : Attribute
	{
		public string Name { get; set; }
	}

	[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
	public sealed class WmiClassAttribute : Attribute
	{
		private string _Ns = "CIMV2";
		public string Namespace
		{
			get { return this._Ns; }
			set { this._Ns = value; }
		}

		public string ClassName { get; set; }
	}
}