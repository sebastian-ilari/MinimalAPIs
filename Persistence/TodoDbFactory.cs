using Microsoft.EntityFrameworkCore;

namespace Persistence;

public static class TodoDbFactory
{
    public static TodoDb Create(string databaseName)
    {
        var options = new DbContextOptionsBuilder<TodoDb>()
            .UseSqlite($"Data Source={databaseName};Mode=Memory;Cache=Shared")
            .Options;

        var context = new TodoDb(options);
        context.Database.OpenConnection();
        context.Database.EnsureCreated();

        return context;
    }

    public static void ClearData(TodoDb context)
    {
        var tables = context.Model.GetEntityTypes()
            .Select(t => t.GetTableName())
            .Distinct()
            .ToList();

        foreach (var table in tables)
        {
            context.Database.ExecuteSqlRaw($"DELETE FROM {table}");
        }
    }

    public static void Dispose(TodoDb context)
    {
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}
