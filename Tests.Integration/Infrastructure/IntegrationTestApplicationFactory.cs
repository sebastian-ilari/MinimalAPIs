using Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Integration.Infrastructure
{
    public class IntegrationTestApplicationFactory : WebApplicationFactory<Startup>
    {
        public void ResetDbConnection()
        {
            Services.GetRequiredService<IntegrationTestConnectionFactory>().ResetConnection();
        }
    }
}
