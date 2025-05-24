// -----------------------------------------------------------------------
//   <copyright file="SagaWithBranchState.cs" company="Not9News">
//       Copyright (c) Not9News. All rights reserved.
//   </copyright>
//  -----------------------------------------------------------------------

using MassTransit;

namespace SagaTests.Sagas;

public class SagaWithRetryState : SagaStateMachineInstance
{
    public string? CurrentState { get; set; }
    public Guid? RetryTokenId { get; set; }

    public int? ContinueInSeconds { get; set; }
    public Guid CorrelationId { get; set; }
}