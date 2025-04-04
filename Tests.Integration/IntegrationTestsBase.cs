using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Persistence;

namespace Tests;

public abstract class IntegrationTestsBase
{
    protected HttpClient _client = null!;
    private WebApplicationFactory<Program> _factory = null!;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<TodoDb>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
                services.AddDbContext<TodoDb>(opt =>
                {
                    opt.UseInMemoryDatabase($"MinimalAPIsTests-{DateTime.Now.ToFileTimeUtc()}");
                });
            });
        });
        _client = _factory.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        //TODO: clear the database
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }
}
