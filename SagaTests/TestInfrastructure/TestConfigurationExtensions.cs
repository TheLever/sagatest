// -----------------------------------------------------------------------
//   <copyright file="TestConfigurationExtensions.cs" company="Not9News">
//       Copyright (c) Not9News. All rights reserved.
//   </copyright>
//  -----------------------------------------------------------------------

using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using SagaTests.Sagas;

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

                x.UsingInMemory((context, cfg) =>
                {
                    cfg.UsePublishMessageScheduler();

                    cfg.ConfigureEndpoints(context);
                });
            });

        return services;
    }

    public static IServiceCollection ConfigureMassTransitWithObserver<TSaga, TState>(this IServiceCollection services,
        TSaga saga,
        TState state,
        IStateObserver<TState> observer)
        where TSaga : MassTransitStateMachine<TState>
        where TState : class, SagaStateMachineInstance
    {
        var repository = new InMemorySagaRepository<TState>();
        services.AddSingleton<IStateObserver<TState>>(observer); // <-- Your observer instance
        services.AddSingleton<TSaga>(saga);
        services.AddSingleton<ISagaRepository<TState>>(repository);
        
        services
            .AddSingleton<ILoggerFactory>(_ => new TestOutputLoggerFactory(TestContext.Current.TestOutputHelper!, true))
            .AddQuartz()
            .AddMassTransitTestHarness(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                x.AddQuartzConsumers();

                x.AddPublishMessageScheduler();

                x.UsingInMemory((context, cfg) =>
                {
                    cfg.UsePublishMessageScheduler();

                    cfg.ConfigureEndpoints(context);
                });
                
                x.AddConfigureEndpointsCallback((name, cfg, context) =>
                {
                    if (cfg != null)
                    {
                        saga.ConnectStateObserver(observer); // <-- Connect your observer here
                    }
                });
            });
        
        return services;
    }
}