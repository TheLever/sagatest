
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;

namespace SagaTests.TestInfrastructure;

public static class TestConfigurationExtensions
{
    public static IServiceCollection ConfigureMassTransit(this IServiceCollection services,
        Action<IBusRegistrationConfigurator>? configure = null)
    {
        services
            .AddSingleton<ILoggerFactory>(_ => new TestOutputLoggerFactory(TestContext.Current.TestOutputHelper!, true))
            .AddQuartz()
            .AddMassTransitTestHarness(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                x.AddQuartzConsumers();

                x.AddPublishMessageScheduler();

                configure?.Invoke(x);


                x.AddConfigureEndpointsCallback((context, name, cfg) =>
                {
                    cfg.UseInMemoryOutbox(context);
                });

                x.UsingInMemory((context, cfg) =>
                {
                    cfg.UsePublishMessageScheduler();
                    cfg.ConfigureEndpoints(context);
                });
            });

        return services;
    }
}