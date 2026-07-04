using CompleteCleanArchitecture.Domain.Entities;

namespace CompleteCleanArchitecture.Application.TodoItems.Dtos;

public sealed record TodoItemDto(Guid Id, string Title, bool IsCompleted, DateTime CreatedAtUtc, DateTime? CompletedAtUtc)
{
    public static TodoItemDto FromEntity(TodoItem item) => new(
        item.Id,
        item.Title,
        item.IsCompleted,
        item.CreatedAtUtc,
        item.CompletedAtUtc);
}
