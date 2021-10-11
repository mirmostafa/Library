﻿using System.Reflection;
using Autofac;

namespace Library.Cqrs;

public static class CqrsModule
{
    public static ContainerBuilder AddCommandQuery(this ContainerBuilder builder, params Assembly[] scannedAssemblies)
    {
        _ = builder.RegisterType<QueryProcessor>()
            .As<IQueryProcessor>()
            .InstancePerLifetimeScope();

        builder
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
            .AsClosedTypesOf(typeof(ICommandValidator<>))
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