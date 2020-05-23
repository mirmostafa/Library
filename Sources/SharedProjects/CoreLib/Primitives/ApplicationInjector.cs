using Mohammad.DesignPatterns.Creational;
using Mohammad.Logging;
using Mohammad.Primitives;
using Mohammad.Settings;

namespace Mohammad.Primitives
{
    public interface IApplicationInjector
    {
        IApplication Application { get; }
        IAppSettings Settings { get; }
        ILogger Logger { get; }
    }

    public abstract class ApplicationInjector<TApplication, TAppSettings, TLogger, TApplicationInjector> : Singleton<TApplicationInjector>, IApplicationInjector
        where TApplicationInjector : ApplicationInjector<TApplication, TAppSettings, TLogger, TApplicationInjector>
        where TApplication : IApplication
        where TAppSettings : IAppSettings
        where TLogger : ILogger
    {
        public TApplication Application { get; private set; }
        public TAppSettings Settings { get; private set; }
        public TLogger Logger { get; private set; }

        public void SetSettings(TAppSettings settings) => this.Settings = settings;
        public void SetLogger(TLogger logger) => this.Logger = logger;
        public void SetApplication(TApplication application) => this.Application = application;

        IApplication IApplicationInjector.Application => this.Application;

        IAppSettings IApplicationInjector.Settings => this.Settings;

        ILogger IApplicationInjector.Logger => this.Logger;
    }

    public class ApplicationInjector<TApplication, TAppSettings> :
        ApplicationInjector<TApplication, TAppSettings, Logger, ApplicationInjector<TApplication, TAppSettings>>
        where TApplication : IApplication where TAppSettings : IAppSettings
    {
        private ApplicationInjector() { this.SetLogger(new Logger()); }
    }
}