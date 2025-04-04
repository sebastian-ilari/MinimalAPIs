using Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

TodoEndpoints.RegisterTodoEndpoints(app);

app.Run();

public partial class Program { }
