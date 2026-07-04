using CompleteCleanArchitecture.Domain.Entities;

namespace CompleteCleanArchitecture.Application.Common.Interfaces;

public interface ITodoItemRepository
{
    Task<TodoItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<TodoItem>> ListAsync(CancellationToken cancellationToken);
    Task AddAsync(TodoItem item, CancellationToken cancellationToken);
}
