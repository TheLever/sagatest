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

                Console.WriteLine(
                    $"Saga {ctx.Saga.CorrelationId}: Received StartDownload, URL: {ctx.Saga.DownloadUrl}, transitioning to Downloading");
            })
            .Activity(x => x.OfInstanceType<DownloadActivity>())
            .TransitionTo(this.Downloading)
        );

        this.During(this.Downloading, this.When(this.DownloadIterationComplete)
            .Then(ctx => Console.WriteLine($"Saga {ctx.Saga.CorrelationId}: Received DownloadIterationComplete"))
            .Respond(ctx =>
            {
                Console.WriteLine(
                    $"Saga {ctx.Saga.CorrelationId}: Sending DownloadComplete for URL: {ctx.Saga.DownloadUrl}");

                return new DownloadComplete(ctx.Saga.CorrelationId, ctx.Saga.DownloadUrl!);
            })
            .Then(ctx =>
                Console.WriteLine($"Saga {ctx.Saga.CorrelationId}: Sent DownloadComplete, transitioning to Completed"))
            .TransitionTo(this.Completed)
        );
    }

    public State? Downloading { get; set; }

    public State? Completed { get; set; }

    public Event<StartDownload>? StartDownload { get; set; }

    public Event<DownloadIterationComplete>? DownloadIterationComplete { get; set; }
}