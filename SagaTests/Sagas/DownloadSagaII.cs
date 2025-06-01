
using MassTransit;
using Microsoft.Extensions.Logging;
using SagaTests.Activities;
using SagaTests.Messages;

namespace SagaTests.Sagas;

public class DownloadSagaII : MassTransitStateMachine<DownloadState>
{
    static DownloadSagaII()
    {
        MessageContracts.Initialize();
    }
    
    public DownloadSagaII(ILogger<DownloadSagaII> log)
    {
        this.InstanceState(x => x.CurrentState);

        this.Initially(this.When(this.StartDownload)
            .Then(ctx =>
            {
                ctx.Saga.CorrelationId = ctx.Message.CorrelationId;
                ctx.Saga.DownloadUrl = ctx.Message.DownloadUrl;
            })
            .Activity(x => x.OfInstanceType<DownloadActivity>())
            .TransitionTo(this.Downloading)
        );

        this.During(this.Downloading, this.When(this.DownloadIterationComplete)
            .RespondAsync(ctx => ctx.Init<DownloadComplete>(new
            {
                ctx.Saga.CorrelationId, 
                ctx.Saga.DownloadUrl,
                RequestId = ctx.RequestId
            }))
            .TransitionTo(this.Completed)
        );
    }

    public State? Downloading { get; set; }

    public State? Completed { get; set; }

    public Event<Messages.StartDownload>? StartDownload { get; set; }

    public Event<DownloadIterationComplete>? DownloadIterationComplete { get; set; }
}