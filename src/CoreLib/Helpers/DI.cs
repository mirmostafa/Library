using Library.Exceptions;
using Library.Validations;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Helpers;

public static class DI
{
    private static IServiceProvider? _serviceProvider = null;

    public static T GetService<T>()
    {
        Check.MustBe(_serviceProvider is not null, () => new LibraryException($"{nameof(DI)} not initiated."));

        var result = _serviceProvider.GetService<T>().NotNull(() => new ObjectNotFoundException($"A service named {typeof(T)} not found."));
        LibLogger.Debug($"Requested service: {typeof(T)}", typeof(DI));
        return result;
    }

    public static void Initialize(in IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider;
}