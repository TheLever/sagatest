// -----------------------------------------------------------------------
//  <copyright file="SagaWithRetry.cs" company="Not9News">
//      Copyright (c) Not9News. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

// using MassTransit;
// using SagaTests.Messages;
//
// namespace SagaTests.Sagas;
//
// public class SagaWithRetry : MassTransitStateMachine<SagaWithRetryState>
// {
//     static SagaWithRetry()
//     {
//         MessageContracts.Initialize();
//     }
//
//     public SagaWithRetry()
//     {
//         this.InstanceState(x => x.CurrentState);
//
//         this.Schedule(() => this.RetrySchedule,
//             i => i.RetryTokenId,
//             s =>
//             {
//                 s.Received = r => r.CorrelateById(ctx => ctx.Message.CorrelationId);
//             });
//
//         this.Initially(this.When(this.StartDownloadEvent)
//             .ThenAsync(async context =>
//             {
//                 Console.WriteLine("Starting download");
//                 await context.Publish<InternalStartDownload>(new InternalStartDownload(context.Message.CorrelationId,
//                     context.Message.ShouldFail));
//             })
//             .TransitionTo(this.Downloading));
//
//         this.During(this.Downloading, 
//             this.When(this.DownloadCompletedEvent)
//                 .Then(context => { Console.WriteLine("Downloading completed"); })
//                 .TransitionTo(this.Completed)
//                 .Finalize(),
//
//             this.When(this.RateLimitedEvent)
//                 .Then(context =>
//                 {
//                     Console.WriteLine("RateLimited");
//                     context.Saga.ContinueInSeconds = context.Message.DelayInSeconds;
//                 })
//                 .TransitionTo(this.RateLimited)
//                 .Schedule(this.RetrySchedule,
//                     ctx => new RetryDownload(ctx.Message.CorrelationId, ctx.Message.CorrelationId),
//                     ctx => TimeSpan.FromSeconds(ctx.Saga.ContinueInSeconds!.Value)));
//
//         this.During(this.RateLimited, this.When(this.RetrySchedule!.Received)
//             .ThenAsync(async context =>
//             {
//                 Console.WriteLine("Reschedule timer expired");
//                 await context.Publish(new InternalStartDownload(context.Message.CorrelationId, false));
//             })
//             .TransitionTo(this.Downloading));
//
//         this.SetCompletedWhenFinalized();
//     }
//
//     #region Schedule
//
//     public Schedule<SagaWithRetryState, RetryDownload> RetrySchedule { get; private set; }
//
//     #endregion
//
//     #region States
//
//     public State? Downloading { get; private set; }
//
//     public State? RateLimited { get; private set; }
//
//     public State? Completed { get; private set; }
//
//     #endregion
//
//     #region Events
//
//     public Event<StartDownload>? StartDownloadEvent { get; private set; }
//
//     public Event<DownloadCompleted>? DownloadCompletedEvent { get; private set; }
//
//     public Event<RateLimitHit>? RateLimitedEvent { get; private set; }
//     
//     #endregion
// }

