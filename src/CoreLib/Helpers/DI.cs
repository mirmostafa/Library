using System.Diagnostics;

using Library.Exceptions;
using Library.Validations;

using Microsoft.Extensions.DependencyInjection;

namespace Library.Helpers;

public static class DI
{
    private static IServiceProvider? _serviceProvider = null;

    [DebuggerStepThrough]
    public static T GetService<T>()
    {
        Check.If(_serviceProvider is not null, () => new LibraryException($"{nameof(DI)} not initiated."));

        LibLogger.Debug($"Requested service: {typeof(T)}", typeof(DI));
        var result = _serviceProvider.GetService<T>().NotNull(() => new ObjectNotFoundException($"A service named {typeof(T)} not found."));
        return result;
    }

    public static void Initialize(in IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider;
}