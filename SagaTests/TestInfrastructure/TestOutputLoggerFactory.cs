
using Microsoft.Extensions.Logging;

namespace SagaTests.TestInfrastructure;

public class TestOutputLoggerFactory(ITestOutputHelper output, bool enabled) : ILoggerFactory
{
    public ITestOutputHelper TestOutputHelper => output;

    public void Dispose()
    {
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new TestOutputLogger(this, enabled);
    }

    public void AddProvider(ILoggerProvider provider)
    {
    }
}