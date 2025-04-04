using NUnit.Framework;

namespace Tests.Integration.Infrastructure;

[SetUpFixture]
public class IntegrationTestSetup
{
    internal static HttpClient Client { get; private set; } = null!;
    internal static IntegrationTestApplicationFactory WebApplicationFactory { get; private set; } = null!;

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        WebApplicationFactory = new IntegrationTestApplicationFactory();
        Client = WebApplicationFactory.CreateClient();
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
        Client.Dispose();
        WebApplicationFactory.Dispose();
    }
}
