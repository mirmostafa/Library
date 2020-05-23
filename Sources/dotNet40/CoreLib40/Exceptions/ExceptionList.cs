#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Collections;
using System.Collections.Generic;

namespace Library40.Exceptions
{
	public class ExceptionList : IEnumerable<ExceptionBase>
	{
		public ExceptionList(IEnumerable<ExceptionBase> exceptions)
		{
			this.Exceptions = exceptions;
		}

		public IEnumerable<ExceptionBase> Exceptions { get; private set; }

		#region IEnumerable<ExceptionBase> Members
		public IEnumerator<ExceptionBase> GetEnumerator()
		{
			return this.Exceptions.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		#endregion
	}
}