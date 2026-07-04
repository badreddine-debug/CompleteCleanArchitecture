using CompleteCleanArchitecture.Application.Common.Exceptions;
using CompleteCleanArchitecture.Application.TodoItems.Commands.CompleteTodoItem;

namespace CompleteCleanArchitecture.Application.Tests.TodoItems;

public sealed class CompleteTodoItemCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenTodoItemDoesNotExist_ShouldThrowNotFoundException()
    {
        var repository = new InMemoryTodoItemRepository();
        var unitOfWork = new FakeUnitOfWork();
        var handler = new CompleteTodoItemCommandHandler(repository, unitOfWork);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(new CompleteTodoItemCommand(Guid.NewGuid()), CancellationToken.None));
    }
}
