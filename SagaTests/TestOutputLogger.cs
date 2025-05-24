// -----------------------------------------------------------------------
//   <copyright file="TestOutputLogger.cs" company="Not9News">
//       Copyright (c) Not9News. All rights reserved.
//   </copyright>
//  -----------------------------------------------------------------------

using Microsoft.Extensions.Logging;

namespace SagaTests;

public class TestOutputLogger(TestOutputLoggerFactory factory, Func<LogLevel, bool> filter)
    : ILogger
{
    private readonly TestOutputLoggerFactory factory = factory;
    private object? scope;

    public TestOutputLogger(TestOutputLoggerFactory factory, bool enabled) : this(factory, _ => enabled)
    {
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!this.IsEnabled(logLevel))
        {
            return;
        }

        var message = formatter(state, exception);
        if (string.IsNullOrEmpty(message))
        {
            return;
        }

        message = $"{DateTime.Now:HH:mm:ss.fff}-{logLevel.ToString()[0]} {message}";
        if (exception != null)
        {
            message += Environment.NewLine + Environment.NewLine + exception;
        }

        this.factory.TestOutputHelper.WriteLine(message);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel != LogLevel.None && filter(logLevel);
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        this.scope = state;
        return TestDisposable.Instance;
    }
}