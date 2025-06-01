// -----------------------------------------------------------------------
//   <copyright file="DownloadIterationComplete.cs" company="Not9News">
//       Copyright (c) Not9News. All rights reserved.
//   </copyright>
//  -----------------------------------------------------------------------

namespace SagaTests.Messages;

public record DownloadIterationComplete(Guid CorrelationId);