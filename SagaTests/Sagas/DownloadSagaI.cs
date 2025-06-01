
using MassTransit;
using SagaTests.Messages;

namespace SagaTests.Sagas;

public class DownloadSagaI : MassTransitStateMachine<DownloadState>
{
    static DownloadSagaI()
    {
        MessageContracts.Initialize();
    }
    
    public DownloadSagaI()
    {
        InstanceState(x => x.CurrentState);

        this.Initially(When(StartDownload)
            .Then(ctx =>
            {
                ctx.Saga.CorrelationId = ctx.Message.CorrelationId;
                ctx.Saga.DownloadUrl = ctx.Message.DownloadUrl;
            })
            .RespondAsync(ctx => ctx.Init<DownloadComplete>(new
            {
                ctx.Saga.CorrelationId,
                DownloadUrl = ctx.Saga.DownloadUrl!
                
            }))
            .TransitionTo(Completed)
        );
    }
    
    #region State
    
    public State? Completed { get; set; }
    
    #endregion
    
    #region Events
    
    public Event<Messages.StartDownload> StartDownload { get; set; }
    
    #endregion
}