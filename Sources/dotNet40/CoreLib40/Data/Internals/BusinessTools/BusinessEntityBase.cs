#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.ComponentModel;
using Library40.ExceptionHandlingPattern;
using Library40.Validation;

namespace Library40.Data.Internals.BusinessTools
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public abstract class BusinessEntityBase
	{
		protected BusinessEntityBase()
		{
			this.Validator = new Validator(this.ExceptionHandling);
		}

		public abstract ExceptionHandling<Exception> ExceptionHandling { get; }

		protected Validator Validator { get; private set; }
	}
}