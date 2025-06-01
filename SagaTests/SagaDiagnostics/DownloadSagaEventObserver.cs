// -----------------------------------------------------------------------
//   <copyright file="DownloadSagaEventObserver.cs" company="Not9News">
//       Copyright (c) Not9News. All rights reserved.
//   </copyright>
//  -----------------------------------------------------------------------

using MassTransit;

namespace SagaTests.SagaDiagnostics;

// public class DownloadSagaEventObserver<TState> : IObserver<BehaviorContext<TState, object>> where TState : class, SagaStateMachineInstance
// {
//     public void OnNext(BehaviorContext<TState, object> context)
//     {
//         Console.WriteLine(
//             $"Event Processed: Saga {context.Saga.CorrelationId}, Event: {context.Event.Name}, State: {context.Saga.CurrentState}");
//     }
//
//     public void OnError(Exception error)
//     {
//         Console.WriteLine($"Error in Saga: {error.Message}");
//     }
//
//     public void OnCompleted()
//     {
//         Console.WriteLine("Saga observer completed.");
//     }
// }