#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.ServiceModel;

namespace Library40.ServiceModel
{
	public static class Exceptioner
	{
		public static void Throw(Exception ex)
		{
			throw new FaultException(new FaultReason(ex.GetBaseException().Message));
		}

		public static void Throw<TFault>(string reason) where TFault : new()
		{
			Throw(() => new TFault(), reason);
		}

		public static void Throw<TFault>(string message, string reason)
		{
			Throw(() => (TFault)typeof (TFault).GetConstructor(new Type[]
			                                                   {
			                                                   }).Invoke(new object[]
			                                                             {
				                                                             message
			                                                             }),
				reason);
		}

		public static void Throw<TFualt>(Func<TFualt> creator, string reason)
		{
			throw new FaultException<TFualt>(creator(), reason);
		}
	}
}