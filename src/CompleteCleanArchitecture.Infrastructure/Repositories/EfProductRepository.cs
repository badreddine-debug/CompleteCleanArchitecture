using CompleteCleanArchitecture.Application.Common.Interfaces;
using CompleteCleanArchitecture.Domain.Entities;
using CompleteCleanArchitecture.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CompleteCleanArchitecture.Infrastructure.Repositories;

public sealed class EfProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public EfProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return _context.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Product>> ListAsync(CancellationToken cancellationToken)
    {
        return await _context.Products.AsNoTracking().ToArrayAsync(cancellationToken);
    }

    public async Task AddAsync(Product product, CancellationToken cancellationToken)
    {
        await _context.Products.AddAsync(product, cancellationToken);
    }
}