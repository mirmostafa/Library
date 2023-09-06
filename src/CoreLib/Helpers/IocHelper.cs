using System.Reflection;

using Library.Ioc;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Helpers;

/// <summary>
/// Provides helper methods for dependency injection.
/// </summary>
public static class IocHelper
{
    public static IServiceCollection AddServices<TService>(this IServiceCollection services, Func<IServiceCollection, Type, IServiceCollection>? install)
        => AddServicesByAssembly<TService>(services, install, Assembly.GetExecutingAssembly());

    public static IServiceCollection AddServices<TMarker, TService>(this IServiceCollection services, Func<IServiceCollection, Type, IServiceCollection>? install)
        => AddServicesByAssembly<TService>(services, install, typeof(TMarker).Assembly);

    public static IServiceCollection AddServices<TService>(this IServiceCollection services, IConfiguration configuration, Func<IServiceCollection, Type, IServiceCollection>? install, params Type[] serviceTyeps)
        => AddServicesByAssembly<TService>(services, install, serviceTyeps.Select(x => x.Assembly).ToArray());

    /// <summary>
    /// Adds services to the specified <see cref="IServiceCollection"/> by the specified assemblies.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="install">The install action.</param>
    /// <param name="assemblies">The assemblies.</param>
    /// <returns>The <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddServicesByAssembly<TService>(this IServiceCollection services,
                                                                     Func<IServiceCollection, Type, IServiceCollection>? install,
                                                                     params Assembly[] assemblies)
    {
        install ??= ServiceCollectionServiceExtensions.AddTransient;
        _ = assemblies.Compact().ForEach(assembly => assembly.DefinedTypes
                                                   .Where(x => typeof(TService).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                                                   .Cast(x => x.AsType())
                                                   .ForEach(x => install(services, x)));
        return services;
    }

    public static IServiceCollection InstallServiceByAssembly<TInstaller>(this IServiceCollection services,
                                                                     IConfiguration configuration,
                                                                     params Assembly[] assemblies)
        where TInstaller : IServiceInstaller
    {
        _ = assemblies.Compact().ForEach(assembly => assembly.DefinedTypes
                                               .Where(x => typeof(TInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                                               .Select(Activator.CreateInstance)
                                               .Cast<TInstaller>()
                                               .Exclude(x => x.Order == int.MinValue)
                                               .OrderBy(x => x.Order ?? int.MaxValue)
                                               .ForEach(x => x.Install()));
        return services;
    }
}