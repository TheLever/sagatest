
namespace SagaTests.TestInfrastructure;

internal class TestDisposable : IDisposable
{
    public static readonly TestDisposable Instance = new();

    public void Dispose()
    {
    }
}