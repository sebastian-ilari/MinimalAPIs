using Microsoft.AspNetCore.Mvc.Testing;
using Models;
using NUnit.Framework;
using Persistence;

namespace Tests.Infrastructure;

[TestFixture]
public abstract class IntegrationTestsBase
{
    protected HttpClient _client = null!;
    private WebApplicationFactory<Program> _factory = null!;
    private TodoDb _context = null!;

    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        _factory = new TestingApplication();
        _client = _factory.CreateClient();
        _context = await TodoDbFactory.Create(DbName.TestDb);
    }

    [TearDown]
    public async Task TearDown()
    {
        await TodoDbFactory.ClearData(_context);
        _context = await TodoDbFactory.Create(DbName.TestDb);
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        _client.Dispose();
        _factory.Dispose();
        await TodoDbFactory.Dispose(_context);
    }
}
