using AutoMapper;
using MariamApp.Data.Entities;
using MariamApp.DTOs.Categories;

namespace MariamApp.Mappings;

public class CategoriesMappingProfile : Profile
{
    public CategoriesMappingProfile()
    {
        CreateMap<CategoryRequest, Category>();
    }
}
