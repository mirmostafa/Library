using System.Reflection;
using Library.Ioc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Helpers;

public static class IocHelper
{
    public static IServiceCollection AddServices<TService>(this IServiceCollection services, Func<IServiceCollection, Type, IServiceCollection>? install)
        => AddServicesByAssembly<TService>(services, install, Assembly.GetExecutingAssembly());

    public static IServiceCollection AddServices<TMarker, TService>(this IServiceCollection services, Func<IServiceCollection, Type, IServiceCollection>? install)
        => AddServicesByAssembly<TService>(services, install, typeof(TMarker).Assembly);

    public static IServiceCollection AddServices<TService>(this IServiceCollection services, IConfiguration configuration, Func<IServiceCollection, Type, IServiceCollection>? install, params Type[] serviceTyeps)
        => AddServicesByAssembly<TService>(services, install, serviceTyeps.Select(x => x.Assembly).ToArray());

    public static IServiceCollection AddServicesByAssembly<TService>(this IServiceCollection services,
                                                                     Func<IServiceCollection, Type, IServiceCollection>? install,
                                                                     params Assembly[] assemblies)
    {
        install ??= ServiceCollectionServiceExtensions.AddTransient;
        _ = assemblies.ForEachItem(assembly => assembly.DefinedTypes
                                                   .Where(x => typeof(TService).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                                                   .Cast(x => x.AsType())
                                                   .ForEachItem(x => install(services, x))
                                                   .Build())
                      .Build();
        return services;
    }

    public static IServiceCollection InstallServiceByAssembly<TInstaller>(this IServiceCollection services,
                                                                     IConfiguration configuration,
                                                                     params Assembly[] assemblies)
        where TInstaller : ILibServiceInstaller
    {
        _ = assemblies.ForEachItem(assembly => assembly.DefinedTypes
                                               .Where(x => typeof(TInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                                               .Select(Activator.CreateInstance)
                                               .Cast<TInstaller>()
                                               .Exclude(x => x.Order == int.MinValue)
                                               .OrderBy(x => x.Order ?? int.MaxValue)
                                               .ForEach(x => x.Install()));
        return services;
    }
}