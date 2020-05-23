#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Globalization;

namespace Library40.Helpers
{
	/// <summary>
	///     A utility to do some common tasks about method
	///     arguments
	/// </summary>
	/// <author>Mohammad</author>
	public static class ArgHelper
	{
		#region Methods

		#region AssertBiggerThan
		internal static void AssertBiggerThan(int arg, int min, string argName)
		{
			if (arg <= min)
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "The argument {0}(={1}) cannot be lass than {2}", argName, arg, min));
		}
		#endregion

		#region AssertNotNull
		public static void AssertNotNull<T>(T arg, string argName) where T : class
		{
			if (arg == null)
				throw new ArgumentNullException(argName);
			if (arg is string && arg.ToString().IsNullOrEmpty())
				throw new ArgumentNullException(argName);
		}
		#endregion

		#endregion
	}
}