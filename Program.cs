using Microsoft.EntityFrameworkCore;
using MinimalAPIs.Endpoints;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("MinimalAPIs"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

TodoItemsEndpoints.RegisterTodoItemsEndpoints(app);

app.Run();

public partial class Program { }
