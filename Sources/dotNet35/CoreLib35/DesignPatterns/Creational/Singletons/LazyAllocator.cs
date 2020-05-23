#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Reflection;

namespace Library35.DesignPatterns.Creational.Singletons
{
	public class LazyAllocator<T> : IAllocator<T>
		where T : class
	{
		// Fields
		private T _Instance;

		// Methods
		private LazyAllocator()
		{
		}

		#region IAllocator<T> Members
		public T Instance
		{
			get
			{
				if (this._Instance == null)
					lock (typeof (T))
					{
						var constructor = typeof (T).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[0], new ParameterModifier[0]);
						if (constructor == null)
							throw new Exception("The object that you want to singleton doesnt have a private/protected constructor so the property cannot be enforced.");
						try
						{
							this._Instance = constructor.Invoke(new object[0]) as T;
						}
						catch (Exception e)
						{
							throw new Exception("The LazySingleton couldnt be constructed, check if the type T has a default constructor", e);
						}
					}
				return this._Instance;
			}
		}
		#endregion

		// Properties

		public void Dispose()
		{
			this._Instance = default(T);
			GC.SuppressFinalize(this);
		}
	}
}