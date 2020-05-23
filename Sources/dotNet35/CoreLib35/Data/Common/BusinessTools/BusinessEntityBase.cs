#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.ComponentModel;
using Library35.ExceptionHandlingPattern;
using Library35.Validation;

namespace Library35.Data.Common.BusinessTools
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