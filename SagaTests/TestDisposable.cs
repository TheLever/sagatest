// -----------------------------------------------------------------------
//   <copyright file="TestDisposable.cs" company="Not9News">
//       Copyright (c) Not9News. All rights reserved.
//   </copyright>
//  -----------------------------------------------------------------------

namespace SagaTests;

internal class TestDisposable : IDisposable
{
    public static readonly TestDisposable Instance = new();

    public void Dispose()
    {
    }
}