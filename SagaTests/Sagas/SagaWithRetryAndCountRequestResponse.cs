// -----------------------------------------------------------------------
//   <copyright file="SagaWithRetryAndCountRequestResponse.cs" company="Not9News">
//       Copyright (c) Not9News. All rights reserved.
//   </copyright>
//  -----------------------------------------------------------------------

using MassTransit;
using SagaTests.Messages;

namespace SagaTests.Sagas;

public class SagaWithRetryAndCountRequestResponse : MassTransitStateMachine<SagaWithRetryAndCountState>
{
    static SagaWithRetryAndCountRequestResponse()
    {
        MessageContracts.Initialize();
    }

    public SagaWithRetryAndCountRequestResponse()
    {
        this.InstanceState(x => x.CurrentState);

        this.Schedule(() => this.RetrySchedule,
            i => i.RetryTokenId,
            s => { s.Received = r => r.CorrelateById(ctx => ctx.Message.CorrelationId); });

        this.Initially(this.When(this.StartDownloadEvent)
            .ThenAsync(async context =>
            {
                await context.Publish(new InternalIterationDownloadStart(context.Message.CorrelationId, false,
                    context.Saga.IterationCount));
            })
            .TransitionTo(this.Downloading));

        // Handle the completion of all iterations
        this.During(this.Downloading,
            this.When(this.DownloadCompletedEvent)
                .TransitionTo(this.Completed)
                .Finalize(),

            // Handle completion of another iteration
            this.When(this.IterationCompleteEvent)
                .ThenAsync(async context =>
                {
                    context.Saga.IterationCount += 1;
                    await context.Publish(new InternalIterationDownloadStart(context.Message.CorrelationId, false,
                        context.Saga.IterationCount));
                }),

            // Handle rate limitation
            this.When(this.RateLimitedEvent)
                .Then(context => { context.Saga.ContinueInSeconds = context.Message.DelayInSeconds; })
                .TransitionTo(this.RateLimited)
                .Schedule(this.RetrySchedule,
                    ctx => new RetryDownload(ctx.Message.CorrelationId, ctx.Message.CorrelationId),
                    ctx => TimeSpan.FromSeconds(ctx.Saga.ContinueInSeconds!.Value)));

        this.During(this.RateLimited, this.When(this.RetrySchedule!.Received)
            .ThenAsync(async context =>
            {
                await context.Publish(new InternalIterationDownloadStart(context.Message.CorrelationId, false,
                    context.Saga.IterationCount));
            })
            .TransitionTo(this.Downloading));

        this.SetCompletedWhenFinalized();
    }

    #region Schedule

    public Schedule<SagaWithRetryAndCountState, RetryDownload> RetrySchedule { get; private set; }

    #endregion

    #region States

    public State? Downloading { get; private set; }

    public State? RateLimited { get; private set; }

    public State? Completed { get; private set; }

    #endregion

    #region Events

    public Event<StartDownload>? StartDownloadEvent { get; private set; }

    public Event<DownloadCompleted>? DownloadCompletedEvent { get; private set; }

    public Event<InternalIterationDownloadCompleted>? IterationCompleteEvent { get; private set; }

    public Event<RateLimitHit>? RateLimitedEvent { get; private set; }

    #endregion
}