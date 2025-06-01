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
    Guid? RequestId { get; set; } // Add RequestId
    Uri? ResponseAddress { get; set; } // Add ResponseAddress
}

public interface DownloadComplete
{
    Guid CorrelationId { get; set; }
    string? DownloadUrl { get; set; }
    Guid? RequestId { get; set; } // Add RequestId
}