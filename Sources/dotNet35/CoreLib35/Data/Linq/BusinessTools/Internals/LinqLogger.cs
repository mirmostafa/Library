#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.IO;
using System.Text;

namespace Library35.Data.Linq.BusinessTools.Internals
{
	internal class LinqLogger : TextWriter
	{
		// Methods
		public override Encoding Encoding
		{
			get { return Encoding.UTF8; }
		}

		public override void Write(object value)
		{
			base.Write(value);
		}

		public override void Write(string value)
		{
			base.Write(value);
		}

		public override void Write(string format, params object[] arg)
		{
			base.Write(format, arg);
		}

		public override void Write(string format, object arg0)
		{
			base.Write(format, arg0);
		}

		public override void Write(string format, object arg0, object arg1)
		{
			base.Write(format, arg0, arg1);
		}

		public override void Write(string format, object arg0, object arg1, object arg2)
		{
			base.Write(format, arg0, arg1, arg2);
		}

		// Properties
	}
}