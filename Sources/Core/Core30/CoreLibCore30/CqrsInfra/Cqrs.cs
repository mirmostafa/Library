using System.Reflection;
using Autofac;
using Mohammad.CqrsInfra.CommandInfra;
using Mohammad.CqrsInfra.QueryInfra;

namespace Mohammad.CqrsInfra
{
    public static class Cqrs
    {
        #region Fields

        private static Assembly _Assembly;

        private static IContainer _container;

        #endregion

        public static void Init(Assembly assembly)
        {
            _Assembly = assembly;
            var cb = new ContainerBuilder();

            AddQueries(cb);
            AddCommands(cb);

            _container = cb.Build();
        }

        public static (IQueryProcessor queryProcessor, ICommandProcessor commandProcessor) GetProcessors()
        {
            var queryProcessor   = _container.Resolve<IQueryProcessor>();
            var commandProcessor = _container.Resolve<ICommandProcessor>();
            return (queryProcessor, commandProcessor);
        }

        private static ContainerBuilder AddQueries(ContainerBuilder builder)
        {
            builder
               .RegisterType<QueryProcessor>()
               .As<IQueryProcessor>()
               .InstancePerLifetimeScope();

            builder
               .RegisterAssemblyTypes(_Assembly)
               .AsClosedTypesOf(typeof(IQueryHandler<,>), "1")
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();

            return builder;
        }

        private static ContainerBuilder AddCommands(ContainerBuilder builder)
        {
            var assembly = _Assembly;
            builder
               .RegisterType<CommandProcessor>()
               .As<ICommandProcessor>()
               .InstancePerLifetimeScope();

            builder
               .RegisterAssemblyTypes(assembly)
               .AsClosedTypesOf(typeof(ICommandHandler<,>), "1")
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();

            builder
               .RegisterAssemblyTypes(assembly)
               .AssignableTo<ICommandResult>()
               .AsImplementedInterfaces()
               .InstancePerRequest();

            builder
               .RegisterAssemblyTypes(assembly)
               .AsClosedTypesOf(typeof(ICommandValidator<>))
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();

            builder
               .RegisterGenericDecorator(typeof(ValidationCommandHandlerDecorator<,>), typeof(ICommandHandler<,>), "1", "2")
               .InstancePerLifetimeScope();

            return builder;
        }
    }
}