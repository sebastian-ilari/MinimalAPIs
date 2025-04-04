using Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;

namespace Tests.Integration.Infrastructure
{
    public class IntegrationTestApplicationFactory : WebApplicationFactory<Startup>
    {
        public void ResetDbConnection()
        {
            Services.GetRequiredService<IntegrationTestConnectionFactory>().ResetConnection();
        }

        protected override IHostBuilder CreateHostBuilder() =>
           Host.CreateDefaultBuilder()
              .ConfigureWebHost(webHostBuilder => {
                  webHostBuilder.UseEnvironment("IntegrationTest");
                  webHostBuilder.UseStartup<Startup>();
              });

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder
               .ConfigureServices(services => {
                   services.AddDbContextFactory<TodoDb>();
                   services.AddSingleton<IntegrationTestConnectionFactory>();
               });
        }
    }
}
