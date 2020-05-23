#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Runtime.Serialization;

namespace Library35.Data.Linq.Exceptions
{
	[Serializable]
	public sealed class InvalidFieldFormatException : DataValidationException
	{
		public InvalidFieldFormatException()
		{
		}

		private InvalidFieldFormatException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}