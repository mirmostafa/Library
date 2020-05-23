#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Runtime.Serialization;

namespace Library40.Exceptions
{
	/// <summary>
	/// </summary>
	[Serializable]
	public class ExceptionBase : Exception
	{
		#region CompanyException
		/// <summary>
		/// </summary>
		public ExceptionBase()
		{
		}
		#endregion

		#region CompanyException
		protected ExceptionBase(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
		#endregion

		#region CompanyException
		/// <summary>
		/// </summary>
		/// <param name="message"></param>
		public ExceptionBase(string message)
			: base(message)
		{
		}
		#endregion

		#region CompanyException
		/// <summary>
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public ExceptionBase(string message, Exception inner)
			: base(message, inner)
		{
		}
		#endregion

		public static void Throw<TCompanyException>() where TCompanyException : ExceptionBase, new()
		{
			Throw(() => new TCompanyException());
		}

		public static void Throw<TCompanyException>(Func<TCompanyException> ctor) where TCompanyException : ExceptionBase
		{
			throw ctor();
		}
	}
}