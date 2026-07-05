using CompleteCleanArchitecture.Application.Common.Exceptions;
using CompleteCleanArchitecture.Application.Common.Interfaces;
using CompleteCleanArchitecture.Application.Products.Dtos;
using MediatR;

namespace CompleteCleanArchitecture.Application.Products.Queries.GetProductById;

public sealed record GetProductByIdQuery(Guid Id) : IRequest<ProductDto>;

public sealed class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    private readonly IProductRepository _repository;

    public GetProductByIdQueryHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Product), request.Id);

        return ProductDto.FromEntity(product);
    }
}