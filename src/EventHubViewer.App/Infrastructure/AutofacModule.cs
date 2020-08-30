using Autofac;
using EventHubViewer.App.Features.Configuration;
using EventHubViewer.App.Infrastructure.Configuration;
using EventHubViewer.App.Infrastructure.Database;
using EventHubViewer.App.Infrastructure.EventHub;
using MediatR;
using Microsoft.Extensions.Hosting;

namespace EventHubViewer.App.Infrastructure
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterDatabase(builder);
            RegisterConfiguration(builder);
            RegisterMediator(builder);
            RegisterSignalRHub(builder);
            RegisterHostedService(builder);
            RegisterMessageProcessor(builder);
        }

        private static void RegisterDatabase(ContainerBuilder builder)
        {
            builder.RegisterType<ConfigurationDatabase>().SingleInstance();
        }

        private static void RegisterConfiguration(ContainerBuilder builder)
        {
            builder.RegisterType<DatabaseConfiguration>().As<IDatabaseConfiguration>().SingleInstance();
        }

        private static void RegisterMediator(ContainerBuilder builder)
        {
            builder.RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            builder.Register<ServiceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            builder.RegisterAssemblyTypes(typeof(GetConfiguration).Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));
        }

        private static void RegisterSignalRHub(ContainerBuilder builder)
        {
            builder.RegisterType<EventHubSignalHub>().SingleInstance();
        }

        private static void RegisterHostedService(ContainerBuilder builder)
        {
            builder.RegisterType<EventHubService>().As<IHostedService>().SingleInstance();
        }

        private static void RegisterMessageProcessor(ContainerBuilder builder)
        {
            builder.RegisterType<MessageProcessor>().SingleInstance();
        }
    }
}