using MassTransit;
using SagaTests.Messages;

namespace SagaTests.Consumers;

public class RepeatedMessageConsumer : IConsumer<InternalIterationDownloadStart>
{
    private const int IterationCount = 4;
    
    public async Task Consume(ConsumeContext<InternalIterationDownloadStart> context)
    {
        if (context.Message.ShouldFail)
        {
            await context.Publish(new RateLimitHit(context.Message.CorrelationId, 7));
        }
        else
        {
            if (context.Message.IterationCount != RepeatedMessageConsumer.IterationCount)
            {
                await context.Publish(new InternalIterationDownloadCompleted(context.Message.CorrelationId, RepeatedMessageConsumer.IterationCount));
            }
            else
            {
                 await context.Publish(new DownloadCompleted(context.Message.CorrelationId));
            }
        }
    }
}