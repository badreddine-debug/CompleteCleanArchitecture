namespace CompleteCleanArchitecture.Infrastructure.Persistence;

public interface IDatabaseInitializer
{
    Task InitializeAsync(CancellationToken cancellationToken = default);
}

public sealed class DatabaseInitializer : IDatabaseInitializer
{
    private readonly ApplicationDbContext _context;

    public DatabaseInitializer(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        return _context.Database.EnsureCreatedAsync(cancellationToken);
    }
}
