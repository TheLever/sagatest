
using MassTransit;

namespace SagaTests.Sagas;

public class DownloadState : SagaStateMachineInstance
{
    public string? CurrentState { get; set; }

    public string? DownloadUrl { get; set; }
    public Guid CorrelationId { get; set; }
}