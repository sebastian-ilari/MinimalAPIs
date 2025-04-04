using NUnit.Framework;
using Tests.Integration.Infrastructure;

namespace Tests.Integration;

public abstract class IntegrationTestsBase
{
    protected HttpClient Client { get; } = IntegrationTestSetup.Client;
    protected IntegrationTestApplicationFactory WebApplicationFactory { get; } = IntegrationTestSetup.WebApplicationFactory;

    //TODO: this constructor should not be necessary, IntegrationTestSetup.RunBeforeAnyTests should take care of it
    protected IntegrationTestsBase()
    {
        WebApplicationFactory = new IntegrationTestApplicationFactory();
        Client = WebApplicationFactory.CreateClient();
    }

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        OneTimeSetup(WebApplicationFactory);
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        OneTimeTearDown(WebApplicationFactory);
    }

    protected virtual void OneTimeSetup(IntegrationTestApplicationFactory factory)
    {
        factory.ResetDbConnection();
    }

    protected virtual void OneTimeTearDown(IntegrationTestApplicationFactory factory)
    {
    }
}
