using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Api;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("MinimalAPIs"));
        services.AddDatabaseDeveloperPageExceptionFilter();
    }
}
