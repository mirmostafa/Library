using Microsoft.Extensions.DependencyInjection;

namespace Library.Cqrs;

public static class DI
{
    private static ServiceProvider? _serviceProvider = null!;

    public static void Initialize(in ServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider;

    public static T? GetService<T>()
    => _serviceProvider is not null ? _serviceProvider.GetService<T>() : default;
}
