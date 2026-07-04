using CompleteCleanArchitecture.Application.Common.Exceptions;
using CompleteCleanArchitecture.Application.Common.Interfaces;
using CompleteCleanArchitecture.Domain.Entities;
using MediatR;

namespace CompleteCleanArchitecture.Application.TodoItems.Commands.CompleteTodoItem;

public sealed record CompleteTodoItemCommand(Guid Id) : IRequest;

public sealed class CompleteTodoItemCommandHandler : IRequestHandler<CompleteTodoItemCommand>
{
    private readonly ITodoItemRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CompleteTodoItemCommandHandler(ITodoItemRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CompleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(TodoItem), request.Id);

        item.Complete();
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
