// -----------------------------------------------------------------------
//   <copyright file="Consumer.cs" company="Not9News">
//       Copyright (c) Not9News. All rights reserved.
//   </copyright>
//  -----------------------------------------------------------------------

using MassTransit;
using SagaTests.Messages;

namespace SagaTests.Consumers;

public class MessageConsumer : IConsumer<InternalStartDownload>
{
    public async Task Consume(ConsumeContext<InternalStartDownload> context)
    {
        if (context.Message.ShouldFail)
        {
            await context.Publish(new RateLimitHit(context.Message.CorrelationId, 7));
        }
        else
        {
            await context.Publish(new DownloadCompleted(context.Message.CorrelationId));
        }
    }
}