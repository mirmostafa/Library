#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Reflection;

namespace Library40.DesignPatterns.Creational.Singletons
{
	/// <summary>
	///     A Singleton using an StaticAllocator used just to simplify the inheritance syntax.
	/// </summary>
	public class Singleton<T>
		where T : class
	{
		private static T _Instance;
		private static object _LockObject;
		protected static Func<T> CreateInstance = () =>
		                                          {
			                                          var constructor = typeof (T).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance,
				                                          null,
				                                          new Type[0],
				                                          new ParameterModifier[0]);
			                                          if (constructor == null)
				                                          throw new Exception("The class must have a private/protected constructor.");
			                                          lock (LockObject)
			                                          {
				                                          return constructor.Invoke(new object[0]) as T;
			                                          }
		                                          };
		protected static object LockObject
		{
			get { return (_LockObject ?? (_LockObject = new object())); }
		}
		public static T Instance
		{
			get { return (_Instance ?? (_Instance = CreateInstance())); }
		}
	}
}