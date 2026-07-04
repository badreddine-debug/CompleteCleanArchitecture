using CompleteCleanArchitecture.Application.Common.Interfaces;
using CompleteCleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CompleteCleanArchitecture.Domain.Entities;

namespace CompleteCleanArchitecture.Application.Tests.TodoItems;

public sealed class CreateTodoItemCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateTodoItem()
    {
        var repository = new InMemoryTodoItemRepository();
        var unitOfWork = new FakeUnitOfWork();
        var handler = new CreateTodoItemCommandHandler(repository, unitOfWork);

        var id = await handler.Handle(new CreateTodoItemCommand("Write tests"), CancellationToken.None);

        var item = await repository.GetByIdAsync(id, CancellationToken.None);
        Assert.NotNull(item);
        Assert.Equal("Write tests", item.Title);
        Assert.Equal(1, unitOfWork.SaveChangesCalls);
    }
}

internal sealed class InMemoryTodoItemRepository : ITodoItemRepository
{
    private readonly List<TodoItem> _items = [];

    public Task<TodoItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return Task.FromResult(_items.FirstOrDefault(item => item.Id == id));
    }

    public Task<IReadOnlyCollection<TodoItem>> ListAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult<IReadOnlyCollection<TodoItem>>(_items.ToArray());
    }

    public Task AddAsync(TodoItem item, CancellationToken cancellationToken)
    {
        _items.Add(item);
        return Task.CompletedTask;
    }
}

internal sealed class FakeUnitOfWork : IUnitOfWork
{
    public int SaveChangesCalls { get; private set; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        SaveChangesCalls++;
        return Task.FromResult(1);
    }
}
