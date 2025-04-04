using Api.Endpoints;
using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("MinimalAPIs"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

TodoEndpoints.RegisterTodoEndpoints(app);

app.Run();

public partial class Program { }
