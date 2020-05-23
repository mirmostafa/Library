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
	/// <typeparam name="TException"> </typeparam>
	public abstract class MultiStepOperationByException<TException> : MultiStepOperation<TException>
		where TException : Exception
	{
	}
}