using CompleteCleanArchitecture.Application.Common.Interfaces;
using CompleteCleanArchitecture.Infrastructure.Persistence;
using CompleteCleanArchitecture.Infrastructure.Repositories;
using CompleteCleanArchitecture.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CompleteCleanArchitecture.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? "Data Source=complete-clean-architecture.db";

        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));
        services.AddScoped<ITodoItemRepository, EfTodoItemRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
        services.AddScoped<ICalculationService, CalculationService>();

        return services;
    }
}
