using Models;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;
using Tests.Infrastructure;

namespace Tests;

[TestFixture]
public class IntegrationTests : IntegrationTestsBase
{
    [Test]
    public async Task GetAllTodos_ReturnsEmptyResult()
    {
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

        var result = await _client.PostAsJsonAsync("/todos", payload);
        var content = await result.Content.ReadFromJsonAsync<TodoDto>();

        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        Assert.That(content?.Id, Is.EqualTo(1));
        Assert.That(content?.Name, Is.EqualTo("Test Todo"));
        Assert.That(content?.IsComplete, Is.True);
    }
}
