#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:59 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.ServiceModel;

namespace Library35.ServiceModel
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
			var constructorInfo = typeof (TFault).GetConstructor(new Type[]
			                                                     {
			                                                     });
			Throw(() => constructorInfo != null
				? (TFault)constructorInfo.Invoke(new object[]
				                                 {
					                                 message
				                                 })
				: default(TFault),
				reason);
		}

		public static void Throw<TFualt>(Func<TFualt> creator, string reason)
		{
			if (creator == null)
				throw new ArgumentNullException("creator");
			throw new FaultException<TFualt>(creator(), reason);
		}
	}
}