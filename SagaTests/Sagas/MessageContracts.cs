
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
        GlobalTopology.Send.UseCorrelationId<DownloadIterationComplete>(x => x.CorrelationId);
        GlobalTopology.Send.UseCorrelationId<DownloadComplete>(x => x.CorrelationId);

        MessageContracts.initialized = true;
    }
}