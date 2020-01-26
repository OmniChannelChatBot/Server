using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Server.Core.Options;
using Server.Infrastructure.Services;
using System;

namespace Server.Api.Extensions
{
    internal static class ServiceCollectionExtension
    {
        private static readonly string _namespaceApplication = $"{nameof(Server)}.{nameof(Api)}.{nameof(Application)}";

        public static void AddApplicationServices(this IServiceCollection services, Action<DBApiOptions> options) => services
            .Configure(options)
            .AddMediatRHandlers()
            .AddFluentValidators()
            .AddIntergationServices();

        private static IServiceCollection AddIntergationServices(this IServiceCollection services)
        {
            services.AddHttpClient<IDbApiServiceClient, DbApiServiceClient>();
            return services;
        }

        private static IServiceCollection AddFluentValidators(this IServiceCollection services) => services
            .Scan(scan => scan
                .FromAssemblies(typeof(Startup).Assembly)
                .AddClasses(classes => classes
                    .InNamespaces(_namespaceApplication)
                    .AssignableTo(typeof(IValidator<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        private static IServiceCollection AddMediatRHandlers(this IServiceCollection services) => services
            .Scan(scan =>
            {
                scan
                    .FromAssemblies(typeof(Startup).Assembly)
                    .AddClasses(classes => classes
                        .InNamespaces(_namespaceApplication)
                        .AssignableTo(typeof(IRequestHandler<>)))
                    .AsImplementedInterfaces()
                    .AddClasses(classes => classes
                        .InNamespaces(_namespaceApplication)
                        .AssignableTo(typeof(IRequestHandler<,>)))
                    .AsImplementedInterfaces()
                    .AddClasses(classes => classes
                        .InNamespaces(_namespaceApplication)
                        .AssignableTo(typeof(IPipelineBehavior<,>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime();
            });
    }
}
