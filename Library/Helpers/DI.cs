using Microsoft.Extensions.DependencyInjection;

namespace Library.Helpers
{
    public static class DI
    {
        private static ServiceProvider _ServiceProvider = null!;

        public static void Initialize(in ServiceProvider serviceProvider)
            => _ServiceProvider = serviceProvider;

        public static T? GetService<T>()
            => _ServiceProvider!.GetService<T>();
    }
}
