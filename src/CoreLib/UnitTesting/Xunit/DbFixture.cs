using Microsoft.Extensions.DependencyInjection;

namespace Library.UnitTesting.Xunit;

public abstract class DbFixtureBase
{
    protected DbFixtureBase()
    {
        var serviceCollection = new ServiceCollection();
        this.OnConfigureServices(serviceCollection);
        this.ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    public ServiceProvider ServiceProvider { get; }

    protected virtual void OnConfigureServices(ServiceCollection services)
    {
    }
}