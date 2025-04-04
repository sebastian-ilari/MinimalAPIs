using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace Tests;

/// <summary>
/// 
/// </summary>
public abstract class IntegrationTestsBase2 : WebApplicationFactory<Program>
{
    protected readonly HttpClient Client;

    public IntegrationTestsBase2(WebApplicationFactory<Program> factory) =>
        Client = factory
            .WithWebHostBuilder(builder => builder.ConfigureServices(services =>
            {
                services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("MinimalAPIs"));
            }))
            .CreateClient();
}
