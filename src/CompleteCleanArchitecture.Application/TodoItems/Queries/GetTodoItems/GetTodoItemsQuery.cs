using CompleteCleanArchitecture.Application.Common.Interfaces;
using CompleteCleanArchitecture.Application.TodoItems.Dtos;
using MediatR;

namespace CompleteCleanArchitecture.Application.TodoItems.Queries.GetTodoItems;

public sealed record GetTodoItemsQuery : IRequest<IReadOnlyCollection<TodoItemDto>>;

public sealed class GetTodoItemsQueryHandler : IRequestHandler<GetTodoItemsQuery, IReadOnlyCollection<TodoItemDto>>
{
    private readonly ITodoItemRepository _repository;

    public GetTodoItemsQueryHandler(ITodoItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<TodoItemDto>> Handle(GetTodoItemsQuery request, CancellationToken cancellationToken)
    {
        var items = await _repository.ListAsync(cancellationToken);

        return items
            .OrderBy(item => item.CreatedAtUtc)
            .Select(TodoItemDto.FromEntity)
            .ToArray();
    }
}
