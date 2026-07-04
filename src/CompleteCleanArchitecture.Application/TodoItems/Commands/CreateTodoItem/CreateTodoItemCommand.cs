using CompleteCleanArchitecture.Application.Common.Interfaces;
using CompleteCleanArchitecture.Domain.Entities;
using FluentValidation;
using MediatR;

namespace CompleteCleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;

public sealed record CreateTodoItemCommand(string Title) : IRequest<Guid>;

public sealed class CreateTodoItemCommandValidator : AbstractValidator<CreateTodoItemCommand>
{
    public CreateTodoItemCommandValidator()
    {
        RuleFor(command => command.Title)
            .NotEmpty()
            .MaximumLength(200);
    }
}

public sealed class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, Guid>
{
    private readonly ITodoItemRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTodoItemCommandHandler(ITodoItemRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var item = TodoItem.Create(request.Title);

        await _repository.AddAsync(item, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return item.Id;
    }
}
