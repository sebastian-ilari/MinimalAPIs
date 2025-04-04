using Microsoft.EntityFrameworkCore;
using Models;
using Persistence;

namespace MinimalAPIs.Endpoints;

public static class TodoEndpoints
{
    public static void RegisterTodoEndpoints(this WebApplication app)
    {
        RouteGroupBuilder todoApp = app.MapGroup("/todos");

        todoApp.MapGet("/", GetAllTodos);
        todoApp.MapGet("/complete", GetCompleteTodos);
        todoApp.MapGet("/{id}", GetTodoById);
        todoApp.MapPost("/", CreateTodo);
        todoApp.MapPut("/{id}", UpdateTodo);
        todoApp.MapDelete("/{id}", DeleteTodo);
    }

    static async Task<IResult> GetAllTodos(TodoDb db)
    {
        return TypedResults.Ok(await db.Todos.ToArrayAsync());
    }

    static async Task<IResult> GetCompleteTodos(TodoDb db)
    {
        return TypedResults.Ok(await db.Todos.Where(t => t.IsComplete).Select(x => new TodoDTO(x)).ToListAsync());
    }

    static async Task<IResult> GetTodoById(int id, TodoDb db)
    {
        return await db.Todos.FindAsync(id)
            is Todo todo
                ? TypedResults.Ok(new TodoDTO(todo))
                : TypedResults.NotFound();
    }

    static async Task<IResult> CreateTodo(TodoDTO todoDTO, TodoDb db)
    {
        var todo = new Todo
        {
            IsComplete = todoDTO.IsComplete,
            Name = todoDTO.Name
        };

        db.Todos.Add(todo);
        await db.SaveChangesAsync();

        todoDTO = new TodoDTO(todo);

        return TypedResults.Created($"/todos/{todo.Id}", todoDTO);
    }

    static async Task<IResult> UpdateTodo(int id, TodoDTO todoDTO, TodoDb db)
    {
        var todo = await db.Todos.FindAsync(id);

        if (todo is null) return TypedResults.NotFound();

        todo.Name = todoDTO.Name;
        todo.IsComplete = todoDTO.IsComplete;

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
