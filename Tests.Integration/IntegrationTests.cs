using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Models;
using NUnit.Framework;
using Persistence;
using System.Net;
using System.Net.Http.Json;

namespace Tests;

[TestFixture]
public class IntegrationTests : IntegrationTestsBase
{
    /*
    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        /*
        var factory = new WebApplicationFactory<Program>();
        _client = factory.CreateClient();
        */

        /*
        var application = new TestingApplication();
        _client = application.CreateClient();
    }
    */

    [Test]
    public async Task GetAllTodos_ReturnsEmptyResult()
    {
        /*
        await using var application = new WebApplicationFactory<Program>();
        _client = application.CreateClient();
        */
        /*
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
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
        _client = factory.CreateClient();
        */

        var response = await _client.GetAsync("/todos");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();

        Assert.That(result, Is.EqualTo("[]"));
    }

    [Test]
    public async Task CreateTodo_ReturnsCreatedItem()
    {
        var payload = new TodoDto(new Todo
        {
            Id = 1,
            Name = "Test Todo",
            IsComplete = true
        });
        await using var application = new WebApplicationFactory<Program>();
        using var client = application.CreateClient();

        var result = await client.PostAsJsonAsync("/todos", payload);
        var content = await result.Content.ReadFromJsonAsync<TodoDto>();

        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        Assert.That(content?.Id, Is.EqualTo(1));
        Assert.That(content?.Name, Is.EqualTo("Test Todo"));
        Assert.That(content?.IsComplete, Is.True);
    }
}
