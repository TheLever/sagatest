using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using SagaTests.Messages;
using SagaTests.Sagas;
using SagaTests.TestInfrastructure;

namespace SagaTests;

public class DownloadSagaITest
{
    [Fact]
    public async Task GivenDownloadSaga_WhenInvoked_WillRespondWithResult()
    {
        // Arrange
        await using var provider = new ServiceCollection()
            .ConfigureMassTransit(x =>
            {
                x.AddSagaStateMachine<DownloadSagaI, DownloadState>()
                    .InMemoryRepository();
                x.AddLogging();
            })
            .BuildServiceProvider(true);
        
        var correlationId = Guid.NewGuid();

        var harness = provider.GetTestHarness();
        var sagaHarness = harness.GetSagaStateMachineHarness<DownloadSagaI, DownloadState>();

        await harness.Start();

        var requestClient = harness.GetRequestClient<Messages.StartDownload>();
        
        // Act
        var response = await requestClient.GetResponse<DownloadComplete>(
            new
            {
                correlationId,
                DownloadUrl = "http://localhost"
            },
            TestContext.Current.CancellationToken);
        
        // Assert
        Assert.NotNull(response);
        Assert.Equal(correlationId, response.Message.CorrelationId);
        Assert.Equal("http://localhost", response.Message.DownloadUrl);
    }
}