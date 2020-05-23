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
	public sealed class HasDataFlowExceptionBase : DataValidationExceptionBase
	{
		// Methods
		public HasDataFlowExceptionBase()
		{
		}

		public HasDataFlowExceptionBase(string message)
			: base(message)
		{
		}

		private HasDataFlowExceptionBase(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public HasDataFlowExceptionBase(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}