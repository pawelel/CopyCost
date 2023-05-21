using CopyCost.Contracts.Repositories;
using CopyCost.Data;
using CopyCost.Repositories;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

namespace CopyCost;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddMudServices();
        services.AddDbContextFactory<CopyCostDbContext>(options =>
        {
            var dataSource = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "CopyCost.db");
            options.UseSqlite($"Data Source={dataSource}");
#if DEBUG
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
            options.LogTo(Console.WriteLine);
#endif
        });

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();

        return services;
    }
}