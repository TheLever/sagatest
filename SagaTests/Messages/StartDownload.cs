// -----------------------------------------------------------------------
//   <copyright file="StartDownload.cs" company="Not9News">
//       Copyright (c) Not9News. All rights reserved.
//   </copyright>
//  -----------------------------------------------------------------------

namespace SagaTests.Messages;

//public record StartDownload(Guid CorrelationId, string DownloadUrl);

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