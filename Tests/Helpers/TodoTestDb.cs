using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Tests.Helpers;

public class TodoTestDb : IDbContextFactory<TodoDb>
{
    public TodoDb CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<TodoDb>()
            .UseInMemoryDatabase($"MinimalAPIsTests-{DateTime.Now.ToFileTimeUtc()}")
            .Options;

        return new TodoDb(options);
    }
}
