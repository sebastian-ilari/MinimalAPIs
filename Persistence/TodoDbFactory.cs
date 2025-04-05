using Microsoft.EntityFrameworkCore;

namespace Persistence;

public static class TodoDbFactory
{
    public async static Task<TodoDb> Create(string databaseName)
    {
        var options = new DbContextOptionsBuilder<TodoDb>()
            .UseSqlite($"Data Source={databaseName};Mode=Memory;Cache=Shared")
            .Options;

        var context = new TodoDb(options);
        await context.Database.OpenConnectionAsync();
        await context.Database.EnsureCreatedAsync();

        return context;
    }

    public async static Task ClearData(TodoDb context)
    {
        var tables = context.Model.GetEntityTypes()
            .Select(t => t.GetTableName())
            .Distinct()
            .ToList();

        foreach (var table in tables)
        {
            await context.Database.ExecuteSqlRawAsync($"DELETE FROM {table}");
        }
    }

    public async static Task Dispose(TodoDb context)
    {
        await context.Database.EnsureDeletedAsync();
        await context.DisposeAsync();
    }
}
