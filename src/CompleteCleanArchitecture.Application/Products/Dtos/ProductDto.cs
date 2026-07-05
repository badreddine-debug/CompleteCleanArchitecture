using CompleteCleanArchitecture.Domain.Entities;

namespace CompleteCleanArchitecture.Application.Products.Dtos;

public sealed record ProductDto(Guid Id, string Name, decimal Price, int Stock, DateTime CreatedAtUtc, DateTime? UpdatedAtUtc)
{
    public static ProductDto FromEntity(Product product) => new(
        product.Id,
        product.Name,
        product.Price,
        product.Stock,
        product.CreatedAtUtc,
        product.UpdatedAtUtc);
}