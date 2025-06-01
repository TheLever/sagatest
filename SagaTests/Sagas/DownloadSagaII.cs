// -----------------------------------------------------------------------
//   <copyright file="DownloadSagaII.cs" company="Not9News">
//       Copyright (c) Not9News. All rights reserved.
//   </copyright>
//  -----------------------------------------------------------------------

using MassTransit;
using Microsoft.Extensions.Logging;
using SagaTests.Activities;
using SagaTests.Messages;

namespace SagaTests.Sagas;

public class DownloadSagaII : MassTransitStateMachine<DownloadState>
{
    public DownloadSagaII(ILogger<DownloadSagaII> log)
    {
        this.InstanceState(x => x.CurrentState);

        this.Event(() => this.StartDownload, x => x.CorrelateById(m => m.Message.CorrelationId));
        
        this.Event(() => this.DownloadIterationComplete, x => x.CorrelateById(m => m.Message.CorrelationId));

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

    public Event<StartDownload>? StartDownload { get; set; }

    public Event<DownloadIterationComplete>? DownloadIterationComplete { get; set; }
}