using System;
using Mohammad.Helpers;

namespace Mohammad.DesignPatterns.Creational
{
    /// <summary>
    ///     A Singleton using an StaticAllocator used just to simplify the inheritance syntax.
    /// </summary>
    public class Singleton<T> : ISingleton<T>
        where T : class, ISingleton<T>
    {
        private static readonly Lazy<T> _Instance = ObjectHelper.GenerateLazySingletonInstance(initializeInstance: InitializeInstance);

        /// <summary>
        ///     The instance initializer
        /// </summary>
        protected static Action<T> InitializeInstance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static T Instance => _Instance.Value;
    }
}