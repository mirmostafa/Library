#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

namespace Library35.DesignPatterns.Creational.Singletons
{
	/// <summary>
	///     A Singleton using an StaticAllocator used just to simplify the inheritance syntax.
	/// </summary>
	public class Singleton<T> : AllocatableSingleton<T, StaticAllocator<T>>
		where T : class
	{
	}
}