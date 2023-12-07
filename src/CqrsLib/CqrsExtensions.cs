using System.Reflection;
using System.Runtime.CompilerServices;

using Autofac;

using Library.Cqrs.Engine.Command;
using Library.Cqrs.Engine.Query;
using Library.Cqrs.Models.Commands;
using Library.Cqrs.Models.Queries;

namespace Library.Cqrs;

public static class CqrsExtensions
{
    [CLSCompliant(false)]
    public static ContainerBuilder AddCqrs(this ContainerBuilder builder, params Assembly[] scannedAssemblies)
    {
        _ = builder.RegisterType<QueryProcessor>()
                .As<IQueryProcessor>()
                .InstancePerLifetimeScope();

        _ = builder
                .RegisterAssemblyTypes(scannedAssemblies)
                .AsClosedTypesOf(typeof(IQueryHandler<,>), "1")
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

        _ = builder
                .RegisterAssemblyTypes(scannedAssemblies)
                .AsClosedTypesOf(typeof(ICommandHandler<,>), "2")
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

        _ = builder
                .RegisterAssemblyTypes(scannedAssemblies)
                .AsClosedTypesOf(typeof(ICommandValidator<>), "3")
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

        _ = builder
                .RegisterType<CommandProcessor>()
                .As<ICommandProcessor>()
                .InstancePerLifetimeScope();

        _ = builder
                .RegisterGenericDecorator(typeof(ValidationCommandHandlerDecorator<,>),
                    typeof(ICommandHandler<,>),
                    "1",
                    "2")
                .InstancePerLifetimeScope();

        return builder;
    }
}
