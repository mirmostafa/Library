#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Runtime.Serialization;

namespace Library40.Data.Common.Exceptions
{
	[Serializable]
	public sealed class MissingRequiredFieldExceptionBase : DataValidationExceptionBase
	{
		public MissingRequiredFieldExceptionBase()
		{
		}

		private MissingRequiredFieldExceptionBase(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}