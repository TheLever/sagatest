
namespace SagaTests.Messages;

public interface StartDownload
{
    Guid CorrelationId { get; }
    string DownloadUrl { get; }
}

public interface DownloadIterationComplete
{
    Guid CorrelationId { get; set; }
}

public interface DownloadComplete
{
    Guid CorrelationId { get; set; }
    string? DownloadUrl { get; set; }
}