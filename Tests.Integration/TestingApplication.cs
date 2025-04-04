using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;

namespace Tests;

public class TestingApplication : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddDbContext<TodoDb>(opt => 
                opt.UseInMemoryDatabase($"MinimalAPIsTests-{DateTime.Now.ToFileTimeUtc()}"));
        });

        return base.CreateHost(builder);
    }
}
