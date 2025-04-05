using MariamApp.Interfaces;
using MariamApp.JWT;
using MariamApp.Services;

namespace MariamApp.Helpers.ServiceExtensions;

public static class AppServicesExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ISupplierService, SupplierService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IReportService, ReportService>();

        services.AddSingleton<JwtGenerator>();
        services.AddSingleton<SignalRService>();

        return services;
    }
}