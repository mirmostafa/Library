﻿using Microsoft.Extensions.DependencyInjection;

namespace Library.Helpers;

public static class DI
{
    private static IServiceProvider? _serviceProvider = null;

    public static void Initialize(in IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider;

    public static T? GetService<T>()
        => _serviceProvider is not null ? _serviceProvider.GetService<T>() : default;
}