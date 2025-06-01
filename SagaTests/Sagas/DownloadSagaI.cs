// -----------------------------------------------------------------------
//   <copyright file="DownloadSagaI.cs" company="Not9News">
//       Copyright (c) Not9News. All rights reserved.
//   </copyright>
//  -----------------------------------------------------------------------

using MassTransit;
using SagaTests.Messages;

namespace SagaTests.Sagas;

public class DownloadSagaI : MassTransitStateMachine<DownloadState>
{
    public DownloadSagaI()
    {
        InstanceState(x => x.CurrentState);

        Event(() => StartDownload, x => x.CorrelateById(m => m.Message.CorrelationId));

        this.Initially(When(StartDownload)
            .Then(ctx =>
            {
                ctx.Saga.CorrelationId = ctx.Message.CorrelationId;
                ctx.Saga.DownloadUrl = ctx.Message.DownloadUrl;
            })
            .Respond(ctx => new DownloadComplete(ctx.Saga.CorrelationId, ctx.Saga.DownloadUrl!))
            .TransitionTo(Completed)
        );
    }
    
    #region State
    
    public State? Downloading { get; set; }
    
    public State? Completed { get; set; }
    
    #endregion
    
    #region Events
    
    public Event<StartDownload> StartDownload { get; set; }
    
    #endregion
}