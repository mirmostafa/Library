#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Reflection;

namespace Library35.DesignPatterns.Creational.Singletons
{
	public class AllocatableSingleton<T, TAllocator>
		where T : class
		where TAllocator : IAllocator<T>
	{
		// Fields
		private static readonly IAllocator<T> _Allocator;

		// Methods
		static AllocatableSingleton()
		{
			var constructor = typeof (TAllocator).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[0], new ParameterModifier[0]);
			if (constructor == null)
				throw new Exception("The allocator that you want to create doesnt have a private/protected constructor.");
			try
			{
				_Allocator = constructor.Invoke(new object[0]) as IAllocator<T>;
			}
			catch (Exception e)
			{
				throw new Exception("The Singleton Allocator couldnt be constructed, check if the type Allocator has a default constructor", e);
			}
		}

		// Properties
		public static T Instance
		{
			get { return _Allocator.Instance; }
		}
	}
}