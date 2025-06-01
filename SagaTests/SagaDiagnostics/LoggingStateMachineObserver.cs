// -----------------------------------------------------------------------
//   <copyright file="LoggingStateMachineObserver.cs" company="Not9News">
//       Copyright (c) Not9News. All rights reserved.
//   </copyright>
//  -----------------------------------------------------------------------

using MassTransit;

namespace SagaTests.SagaDiagnostics;

public class LoggingStateMachineObserver<TSaga> : IStateObserver<TSaga>
    where TSaga : class, SagaStateMachineInstance
{
    public Task StateChanged(BehaviorContext<TSaga> context, State currentState, State previousState)
    {
        throw new NotImplementedException();
    }
}