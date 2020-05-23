#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;

namespace Library35.Globalization
{
	public class DateTimeFormatter : IFormatProvider, ICustomFormatter
	{
		#region ICustomFormatter Members
		public string Format(string format, object arg, IFormatProvider formatProvider)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region IFormatProvider Members
		public object GetFormat(Type formatType)
		{
			return formatType == typeof (ICustomFormatter) ? this : null;
		}
		#endregion
	}
}