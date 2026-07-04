using CompleteCleanArchitecture.Application;
using CompleteCleanArchitecture.Application.Common.Exceptions;
using CompleteCleanArchitecture.Application.TodoItems.Commands.CompleteTodoItem;
using CompleteCleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CompleteCleanArchitecture.Application.TodoItems.Queries.GetTodoItems;
using CompleteCleanArchitecture.Infrastructure;
using CompleteCleanArchitecture.Infrastructure.Persistence;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
    await initializer.InitializeAsync();
}

var todoItems = app.MapGroup("/api/todo-items")
    .WithTags("TodoItems")
    .WithOpenApi();

todoItems.MapGet("/", async (ISender sender, CancellationToken cancellationToken) =>
{
    var result = await sender.Send(new GetTodoItemsQuery(), cancellationToken);
    return Results.Ok(result);
});

todoItems.MapPost("/", async (CreateTodoItemRequest request, ISender sender, CancellationToken cancellationToken) =>
{
    try
    {
        var id = await sender.Send(new CreateTodoItemCommand(request.Title), cancellationToken);
        return Results.Created($"/api/todo-items/{id}", new { id });
    }
    catch (ValidationException exception)
    {
        return Results.ValidationProblem(exception.Errors);
    }
});

todoItems.MapPut("/{id:guid}/complete", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
{
    try
    {
        await sender.Send(new CompleteTodoItemCommand(id), cancellationToken);
        return Results.NoContent();
    }
    catch (NotFoundException exception)
    {
        return Results.NotFound(new { error = exception.Message });
    }
});

app.Run();

public sealed record CreateTodoItemRequest(string Title);
