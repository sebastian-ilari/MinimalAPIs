﻿using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models;
using Persistence;

namespace Tests.Infrastructure;

public class TestingApplication : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddDbContext<TodoDb>(opt =>
                opt.UseSqlite($"Data Source={DbName.TestDb};Mode=Memory;Cache=Shared"));
        });

        return base.CreateHost(builder);
    }
}
