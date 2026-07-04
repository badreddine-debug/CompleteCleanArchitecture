using CompleteCleanArchitecture.Application.Common.Interfaces;
using CompleteCleanArchitecture.Domain.Entities;
using CompleteCleanArchitecture.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CompleteCleanArchitecture.Infrastructure.Repositories;

public sealed class EfTodoItemRepository : ITodoItemRepository
{
    private readonly ApplicationDbContext _context;

    public EfTodoItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<TodoItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return _context.TodoItems.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<TodoItem>> ListAsync(CancellationToken cancellationToken)
    {
        return await _context.TodoItems.AsNoTracking().ToArrayAsync(cancellationToken);
    }

    public async Task AddAsync(TodoItem item, CancellationToken cancellationToken)
    {
        await _context.TodoItems.AddAsync(item, cancellationToken);
    }
}
