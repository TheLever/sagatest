// -----------------------------------------------------------------------
//   <copyright file="StateMachineTestFixture.cs" company="Not9News">
//       Copyright (c) Not9News. All rights reserved.
//   </copyright>
//  -----------------------------------------------------------------------

using System.Diagnostics;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Xunit.Abstractions;

namespace SagaTests;

public class StateMachineTestFixture<TStateMachine, TInstance>(ITestOutputHelper output) : IAsyncLifetime
    where TStateMachine : class, SagaStateMachine<TInstance>
    where TInstance : class, SagaStateMachineInstance
{
    protected ISagaStateMachineTestHarness<TStateMachine, TInstance>? SagaHarness;
    private ServiceProvider? serviceProvider;
    protected TStateMachine? StateMachine;
    protected ITestHarness? TestHarness;
    
    private TimeSpan testOffset;

    public async Task InitializeAsync()
    {
        this.InterceptQuartzSystemTime();

        var collection = new ServiceCollection()
            .AddSingleton<ILoggerFactory>(provider => new TestOutputLoggerFactory(output, true))
            .AddQuartz()
            .AddMassTransitTestHarness(cfg =>
            {
                cfg.SetKebabCaseEndpointNameFormatter();
                cfg.AddQuartzConsumers();
                
                cfg.AddSagaStateMachine<TStateMachine, TInstance>()
                    .InMemoryRepository();
                
                cfg.AddConsumers(typeof(TStateMachine).Assembly);
                
                cfg.AddPublishMessageScheduler();
                cfg.AddDelayedMessageScheduler();

                cfg.UsingInMemory((context, x) =>
                {
                    x.UsePublishMessageScheduler();

                    x.ConfigureEndpoints(context);
                });
                
                cfg.SetTestTimeouts(Debugger.IsAttached ? TimeSpan.FromMinutes(10) : TimeSpan.FromSeconds(30));
            });

        this.ConfigureServices(collection);

        this.serviceProvider = collection.BuildServiceProvider(true);

        this.ConfigureLogging();

        this.TestHarness = this.serviceProvider.GetRequiredService<ITestHarness>();

        await this.TestHarness.Start();

        this.SagaHarness =
            this.serviceProvider.GetRequiredService<ISagaStateMachineTestHarness<TStateMachine, TInstance>>();
        this.StateMachine = this.serviceProvider.GetRequiredService<TStateMachine>();
    }

    public async Task DisposeAsync()
    {
        try
        {
            await this.TestHarness?.Stop()!;
        }
        finally
        {
            this.serviceProvider?.DisposeAsync();
        }
    }

    protected Task AdvanceSystemTime(TimeSpan duration)
    {
        return Task.CompletedTask;
        // var scheduler = await this.schedulerFactory!.GetScheduler().ConfigureAwait(false);
        // await scheduler.Standby().ConfigureAwait(false);
        // this.testOffset += duration;
        //
        // await scheduler.Start().ConfigureAwait(false);
    }

    private void ConfigureLogging()
    {
        var loggerFactory = this.serviceProvider!.GetRequiredService<ILoggerFactory>();
        LogContext.ConfigureCurrentLogContext(loggerFactory);
        Quartz.Logging.LogContext.SetCurrentLogProvider(loggerFactory);
    }

    private void InterceptQuartzSystemTime()
    {
        SystemTime.UtcNow = () => DateTimeOffset.UtcNow;
        SystemTime.Now = () => DateTimeOffset.Now;
    }

    private static void RestoreDefaultQuartzSystemTime()
    {
        SystemTime.UtcNow = () => DateTimeOffset.UtcNow;
        SystemTime.Now = () => DateTimeOffset.Now;
    }
    
    private DateTimeOffset GetUtcNow()
    {
        return DateTimeOffset.UtcNow + this.testOffset;
    }

    private DateTimeOffset GetNow()
    {
        return DateTimeOffset.UtcNow + this.testOffset;
    }

    protected virtual void ConfigureServices(IServiceCollection services)
    {
    }
}