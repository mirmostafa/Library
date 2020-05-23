#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:05 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;

namespace Library40.Validation.Attributes
{
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class NotLessThanAttribute : ValidationAttribute
	{
		public NotLessThanAttribute(long minValue)
		{
			this.MinValue = minValue;
		}

		public long MinValue { get; private set; }
	}
}