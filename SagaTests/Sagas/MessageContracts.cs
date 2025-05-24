// -----------------------------------------------------------------------
//   <copyright file="MessageContracts.cs" company="Not9News">
//       Copyright (c) Not9News. All rights reserved.
//   </copyright>
//  -----------------------------------------------------------------------

using MassTransit;
using SagaTests.Messages;

namespace SagaTests.Sagas;

public static class MessageContracts
{
    private static bool initialized;

    public static void Initialize()
    {
        if (MessageContracts.initialized)
        {
            return;
        }

        GlobalTopology.Send.UseCorrelationId<StartDownload>(x => x.CorrelationId);
        GlobalTopology.Send.UseCorrelationId<RetryDownload>(x => x.CorrelationId);
        GlobalTopology.Send.UseCorrelationId<InternalStartDownload>(x => x.CorrelationId);
        GlobalTopology.Send.UseCorrelationId<RateLimitHit>(x => x.CorrelationId);
        
        MessageContracts.initialized = true;
    }
}