#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;

namespace Library35.Bcl
{
	/// <summary>
	/// </summary>
	public abstract class MultiStepOperation : MultiStepOperation<Exception>
	{
		protected override Exception GenerateException(Exception ex)
		{
			return new Exception(string.Empty, ex);
		}
	}
}