#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;

namespace Library40.Globalization
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