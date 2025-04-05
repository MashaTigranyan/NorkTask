using MariamApp.Mappings;

namespace MariamApp.Helpers.ServiceExtensions;

public static class AutoMapperExtensions
{
    public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(CategoriesMappingProfile));
        services.AddAutoMapper(typeof(SuppliersMappingProfile));
        services.AddAutoMapper(typeof(ProductsMappingProfile));

        return services;
    }
}