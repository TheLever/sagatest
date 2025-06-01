// -----------------------------------------------------------------------
//   <copyright file="Messages.cs" company="Not9News">
//       Copyright (c) Not9News. All rights reserved.
//   </copyright>
//  -----------------------------------------------------------------------

namespace SagaTests.Messages;

//public record StartDownload(Guid CorrelationId, bool ShouldFail);

public record DownloadCompleted(Guid CorrelationId);

public record RetryDownload(Guid CorrelationId, Guid DownloadId);

public record InternalStartDownload(Guid CorrelationId, bool ShouldFail);

public record InternalIterationDownloadStart(Guid CorrelationId, bool ShouldFail, int IterationCount);

public record InternalIterationDownloadCompleted(Guid CorrelationId, int IterationCount);

public record RateLimitHit(Guid CorrelationId, int DelayInSeconds);