// -----------------------------------------------------------------------
//   <copyright file="SagaStateMachineWithBranchTest.cs" company="Not9News">
//       Copyright (c) Not9News. All rights reserved.
//   </copyright>
//  -----------------------------------------------------------------------

namespace SagaTests;

// public class SagaStateMachineWithBranchTest(ITestOutputHelper output)
//     : StateMachineTestFixture<SagaWithRetry, SagaWithRetryState>(output)
// {
//     [Fact]
//     public async Task GivenSagaWithoutRateLimitation_WhenInvoked_ThenSagaCompletes()
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
//         Assert.True(await SagaHarness!.Consumed.Any<StartDownload>(), "Message StartDownload was not consumed by saga");
//         Assert.False(await SagaHarness!.Consumed.Any<RateLimitHit>(), "Message RateLimitHit should not be consumed by saga");
//         
//         var notExists = await this.SagaHarness.NotExists(correlationId);
//         Assert.False(notExists.HasValue);
//         
//         var instance =
//             this.SagaHarness.Created.ContainsInState(correlationId, this.StateMachine, this.StateMachine!.Completed);
//         Assert.True(instance is null, "Instance should be null");
//     }
//     
//     [Fact]
//     public async Task GivenSagaWithRateLimitation_WhenInvoked_ThenSagaCompletes()
//     {
//         var correlationId = Guid.NewGuid();
//
//         // Act
//         await this.TestHarness!.Bus.Publish(new StartDownload(correlationId, true));
//
//         // Assert
//         Assert.True(await this.TestHarness.Consumed.Any<StartDownload>(), "Message StartDownload was not consumed by harness");
//
//         Assert.True(await SagaHarness!.Consumed.Any<StartDownload>(), "Message StartDownload was not consumed by saga");
//         Assert.True(await SagaHarness!.Consumed.Any<RateLimitHit>(), "Message RateLimitHit was not consumed by saga");
//         
//         var notExists = await this.SagaHarness.NotExists(correlationId);
//         Assert.False(notExists.HasValue);
//         
//         var instance =
//             this.SagaHarness.Created.ContainsInState(correlationId, this.StateMachine, this.StateMachine!.Completed);
//         Assert.True(instance is null, "Instance should be null");
//     }
// }
//
// public class SagaStateMachineWithRetryAndCountRequestResponseTest(ITestOutputHelper output) : StateMachineTestFixture<
//     SagaWithRetryAndCountRequestResponse, SagaWithRetryAndCountState>(output)
// {
// }