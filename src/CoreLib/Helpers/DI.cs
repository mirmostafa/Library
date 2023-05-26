using System.Diagnostics;

using Library.Exceptions;
using Library.Validations;

using Microsoft.Extensions.DependencyInjection;

namespace Library.Helpers;

/// <summary>
/// DI static class provides a way to get services from the service provider.
/// </summary>
public static class DI
{
    private static IServiceProvider? _serviceProvider = null;

    /// <summary>
    /// Gets the service of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the service.</typeparam>
    /// <returns>The service of the specified type.</returns>
    /// <exception cref="LibraryException">Thrown when DI is not initiated.</exception>
    /// <exception cref="ObjectNotFoundException">Thrown when unable to resolve service for the specified type.</exception>
    [DebuggerStepThrough]
    public static T GetService<T>()
    {
        Check.If(_serviceProvider is not null, () => new LibraryException($"{nameof(DI)} not initiated."));

        LibLogger.Debug($"Requested service: {typeof(T)}", typeof(DI));
        var result = _serviceProvider.GetService<T>().NotNull(() => new ObjectNotFoundException($"Unable to resolve service for type {typeof(T)}."));
        return result;
    }

    /// <summary>
    /// Initializes the DI class.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    public static void Initialize(in IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;
}