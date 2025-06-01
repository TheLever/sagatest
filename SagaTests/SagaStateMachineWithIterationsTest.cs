// -----------------------------------------------------------------------
//   <copyright file="SagaStateMachineWithIterationsTest.cs" company="Not9News">
//       Copyright (c) Not9News. All rights reserved.
//   </copyright>
//  -----------------------------------------------------------------------

namespace SagaTests;

// public class SagaStateMachineWithIterationsTest(ITestOutputHelper output)
//     : StateMachineTestFixture<SagaWithRetryAndCount, SagaWithRetryAndCountState>(output)
// {
//     [Fact]
//     public async Task GivenSagaWithSeveralIterations_WhenInvoked_ThenAllIterationsAreCompleted()
//     {
//         // Arrange
//         var correlationId = Guid.NewGuid();
//         
//         // Act 
//         await this.TestHarness!.Bus.Publish(new StartDownload(correlationId, false));
//         
//         // Assert
//         Assert.True(await this.TestHarness.Consumed.Any<StartDownload>(), "Message StartDownload was not consumed by harness");
//
//         Assert.True(await this.SagaHarness!.Consumed.Any<StartDownload>(), "Message StartDownload was not consumed by saga");
//         Assert.False(await this.SagaHarness!.Consumed.Any<RateLimitHit>(), "Message RateLimitHit should not be consumed by saga");
//         Assert.True(await this.SagaHarness!.Consumed.Any<InternalIterationDownloadCompleted>(), "Message InternalIterationDownloadCompleted was not consumed by saga");
//         
//         var notExists = await this.SagaHarness.NotExists(correlationId);
//         Assert.False(notExists.HasValue);
//         
//         var instance =
//             this.SagaHarness.Created.ContainsInState(correlationId, this.StateMachine, this.StateMachine!.Completed);
//         Assert.True(instance is null, "Instance should be null");
//     }
// }