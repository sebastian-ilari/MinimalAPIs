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
    public void OneTimeSetup()
    {
        _factory = new TestingApplication();

        _client = _factory.CreateClient();

        _context = TodoDbFactory.Create(DbName.TestDb);
    }

    [TearDown]
    public void TearDown()
    {
        TodoDbFactory.ClearData(_context);
        _context = TodoDbFactory.Create(DbName.TestDb);
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _client.Dispose();
        _factory.Dispose();
        TodoDbFactory.Dispose(_context);
    }
}
