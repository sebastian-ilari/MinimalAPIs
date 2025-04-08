using Microsoft.EntityFrameworkCore;
using Models;
using Persistence;
using Services;

namespace Api.Endpoints;

public static class TodoEndpoints
{
    public static void RegisterTodoEndpoints(this WebApplication app)
    {
        RouteGroupBuilder todoApp = app.MapGroup("/todos").MapTodosApi();
    }

    static RouteGroupBuilder MapTodosApi(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetAllTodos);
        group.MapGet("/complete", GetCompleteTodos);
        group.MapGet("/{id:int}", GetTodoById);
        group.MapPost("/", CreateTodo);
        group.MapPost("/batch", CreateTodoBatch);
        group.MapPut("/{id:int}", UpdateTodo);
        group.MapDelete("/{id:int}", DeleteTodo);

        return group;
    }

    static async Task<IResult> GetAllTodos(TodoDb db)
    {
        return TypedResults.Ok(await db.Todos.Select(x => new TodoDto(x)).ToArrayAsync());
    }

    static async Task<IResult> GetCompleteTodos(TodoDb db, CancellationToken cancellationToken)
    {
        return TypedResults.Ok(await db.Todos.Where(t => t.IsComplete).Select(x => new TodoDto(x))
            .ToListAsync(cancellationToken));
    }

    static async Task<IResult> GetTodoById(int id, TodoDb db)
    {
        return await db.Todos.FindAsync(id)
            is Todo todo
                ? TypedResults.Ok(new TodoDto(todo))
                : TypedResults.NotFound();
    }

    static async Task<IResult> CreateTodo(TodoDto todoDto, TodoDb db, ISecretService secretService)
    {
        var todo = new Todo
        {
            IsComplete = todoDto.IsComplete,
            Name = todoDto.Name,
            Secret = secretService.GetSecret()
        };

        db.Todos.Add(todo);
        await db.SaveChangesAsync();

        todoDto = new TodoDto(todo);

        return TypedResults.Created($"/todos/{todo.Id}", todoDto);
    }

    static async Task<IResult> CreateTodoBatch(TodoDto[] todoDtos, TodoDb db, ISecretService secretService)
    {
        var secret = secretService.GetSecret();
        var todos = todoDtos.Select(t => new Todo
        {
            IsComplete = t.IsComplete,
            Name = t.Name,
            Secret = secret
        });

        await db.Todos.AddRangeAsync(todos);
        await db.SaveChangesAsync();

        return TypedResults.Created($"/todos", todoDtos);
    }

    static async Task<IResult> UpdateTodo(int id, TodoDto todoDto, TodoDb db)
    {
        var todo = await db.Todos.FindAsync(id);

        if (todo is null) return TypedResults.NotFound();

        todo.Name = todoDto.Name;
        todo.IsComplete = todoDto.IsComplete;

        await db.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    static async Task<IResult> DeleteTodo(int id, TodoDb db)
    {
        if (await db.Todos.FindAsync(id) is Todo todo)
        {
            db.Todos.Remove(todo);
            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }

        return TypedResults.NotFound();
    }
}
