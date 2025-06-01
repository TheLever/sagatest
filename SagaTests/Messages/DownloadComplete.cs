// -----------------------------------------------------------------------
//   <copyright file="DownloadComplete.cs" company="Not9News">
//       Copyright (c) Not9News. All rights reserved.
//   </copyright>
//  -----------------------------------------------------------------------

namespace SagaTests.Messages;

public record DownloadComplete(Guid CorrelationId, string DownloadUrl);