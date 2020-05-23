#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

namespace Library35.DesignPatterns.Creational.Singletons
{
	/// <summary>
	///     A LazySingleton implementation using a LazyAllocator just to simplify the syntax of the Singleton inheritance.
	/// </summary>
	public class LazyAllocatableSingleton<T> : AllocatableSingleton<T, LazyAllocator<T>>
		where T : class
	{
	}
}