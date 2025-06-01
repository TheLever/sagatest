// -----------------------------------------------------------------------
//   <copyright file="DownloadActivity.cs" company="Not9News">
//       Copyright (c) Not9News. All rights reserved.
//   </copyright>
//  -----------------------------------------------------------------------

using MassTransit;
using Microsoft.Extensions.Logging;
using SagaTests.Messages;
using SagaTests.Sagas;

namespace SagaTests.Activities;

public class DownloadActivity(ILogger<DownloadActivity> log) : IStateMachineActivity<DownloadState>
{
    public void Probe(ProbeContext context)
    {
        context.CreateScope("download");
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<DownloadState> context, IBehavior<DownloadState> next)
    {
        await this.ExecuteDownload(context);
        await next.Execute(context);
    }

    public async Task Execute<T>(BehaviorContext<DownloadState, T> context, IBehavior<DownloadState, T> next)
        where T : class
    {
        await this.ExecuteDownload(context);
        await next.Execute(context);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<DownloadState, TException> context,
        IBehavior<DownloadState> next) where TException : Exception
    {
        return next.Faulted(context);
    }

    public Task Faulted<T, TException>(BehaviorExceptionContext<DownloadState, T, TException> context,
        IBehavior<DownloadState, T> next) where T : class where TException : Exception
    {
        return next.Faulted(context);
    }

    private async Task ExecuteDownload(BehaviorContext<DownloadState> context)
    {
        log.LogWarning($"[DownloadActivity.ExecuteDownload] context message correlationId: {context.Saga.CorrelationId} {context.Saga.CurrentState}");
        
        var consumeContext = context.GetPayload<ConsumeContext>();
        
        log.LogWarning($"[DownloadActivity.ExecuteDownload] publishing DownloadIterationComplete {context.Saga.CorrelationId} {context.Saga.CurrentState}");
        // Publish completion event after work is done
        await consumeContext.Publish<DownloadIterationComplete>(new
        {
            context.Saga.CorrelationId,
            RequestId = context.RequestId, // Explicitly pass RequestId
            ResponseAddress = consumeContext.ResponseAddress // Preserve response address
        });
    }
}