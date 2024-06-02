using System.Reflection;

using Library.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace CoreLib.BusinessServices;

public static class ServiceHelper
{
    public static TServiceProvider Initialize<TServiceProvider>(this TServiceProvider serviceProvider)
        where TServiceProvider : IServiceProvider
    {
        DI.Initialize(serviceProvider);
        return serviceProvider;
    }

    public static IServiceCollection RegisterServices<TService>(this IServiceCollection services, Assembly assembly)
            => services.RegisterServices<TService>(assembly, assembly);

    /// <summary>
    /// Registers services of type TService from the specified interface and service assemblies.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <param name="services">The services.</param>
    /// <param name="interfaceModule">The interface module.</param>
    /// <param name="serviceModule">The service module.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection RegisterServices<TService>(this IServiceCollection services, Type interfaceModule, Type serviceModule)
            => services.RegisterServices<TService>(interfaceModule.Assembly, serviceModule.Assembly);

    /// <summary>
    /// Registers services from two assemblies based on a given interface.
    /// </summary>
    /// <typeparam name="TServiceInterface">The interface to register services for.</typeparam>
    /// <param name="serviceCollection">The service collection to add services to.</param>
    /// <param name="interfaceAsm">The assembly containing the interface.</param>
    /// <param name="serviceAsm">The assembly containing the services.</param>
    /// <param name="add">An optional action to add services to the service collection.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection RegisterServices<TServiceInterface>(
            this IServiceCollection serviceCollection,
            in Assembly interfaceAsm,
            in Assembly serviceAsm,
            in Action<(IServiceCollection ServiceCollection, Type ServiceInterface, Type ServiceType)>? add = default)
    {
        //Declare a function to add services to the ServiceCollection
        var addToServices = add ?? ((x) => _ = x.ServiceCollection.AddScoped(x.ServiceInterface, x.ServiceType));

        //Get a list of all interfaces that implement TServiceInterface
        var interfaces = interfaceAsm.GetTypes().Where(t => t.IsInterface && t.GetInterfaces().Contains(typeof(TServiceInterface))).ToList();

        //Get a list of all classes that implement TServiceInterface
        var services = serviceAsm.GetTypes().Where(t => t.IsClass && t.GetInterfaces().Contains(typeof(TServiceInterface))).ToList();

        //Loop through each interface and check if there is a corresponding service
        foreach (var iface in interfaces)
        {
            foreach (var svc in services)
            {
                //If a service is found, add it to the ServiceCollection
                if (svc.GetInterface(iface.Name) != null)
                {
                    addToServices((serviceCollection, iface, svc));
                    break;
                }
            }
        }

        //Return the ServiceCollection
        return serviceCollection;
    }

    /// <summary>
    /// Registers services with IService from the assembly of the specified type.
    /// </summary>
    /// <typeparam name="TStartup">The type of the startup.</typeparam>
    /// <param name="services">The services.</param>
    /// <returns>The services.</returns>
    public static IServiceCollection RegisterServicesWithIService<TStartup>(this IServiceCollection services)
            => services.RegisterServices<IService>(typeof(TStartup).Assembly);
}