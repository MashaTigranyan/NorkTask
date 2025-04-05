using AutoMapper;
using MariamApp.Data.Entities;
using MariamApp.DTOs.Products;

namespace MariamApp.Mappings;

public class ProductsMappingProfile : Profile
{
    public ProductsMappingProfile()
    {
        CreateMap<ProductRequest, Product>();
    }
}