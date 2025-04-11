using Api.Endpoints;
using Microsoft.EntityFrameworkCore;
using Models;
using Persistence;
using Services;

var builder = WebApplication.CreateBuilder(args);

// If I use a different type of database, then I would have to unregister the DbContextOption and TodoDb sservices
// in TestingApplication.
//builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase(DbNames.ApplicationDb));

builder.Services.AddDbContext<TodoDb>(options =>
{
    options.UseSqlite($"Data Source={DbName.ApplicationDb};Mode=Memory;Cache=Shared");
});
await TodoDbFactory.Create(DbName.ApplicationDb);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddTransient<ISecretService, SecretService>();

builder.Services.ConfigureHttpJsonOptions(options => {
    options.SerializerOptions.WriteIndented = true;
    options.SerializerOptions.IncludeFields = true;
});

var app = builder.Build();

TodoEndpoints.RegisterTodoEndpoints(app);

app.Run();

public partial class Program { }
