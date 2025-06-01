// -----------------------------------------------------------------------
//   <copyright file="StartDownload.cs" company="Not9News">
//       Copyright (c) Not9News. All rights reserved.
//   </copyright>
//  -----------------------------------------------------------------------

namespace SagaTests.Messages;

public record StartDownload(Guid CorrelationId, string DownloadUrl);