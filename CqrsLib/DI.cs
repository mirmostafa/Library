using Microsoft.Extensions.DependencyInjection;
using System;

namespace Library.Cqrs;

public static class DI
{
    private static IServiceProvider? _serviceProvider = null!;

    public static void Initialize(in IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider;

    public static T? GetService<T>()
    => _serviceProvider is not null ? _serviceProvider.GetService<T>() : default;
}
