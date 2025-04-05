using Microsoft.EntityFrameworkCore;
using db = MariamApp.Data;

namespace MariamApp.Helpers.ServiceExtensions;

public static class DatabaseExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("app-db");

        services.AddDbContext<db.DataContext>(options => options.UseSqlServer(connectionString));
        services.AddDbContext<db.AppUsersDbContext>(options => options.UseSqlServer(connectionString));

        return services;
    }
}