using AutoMapper;
using MariamApp.Data.Entities;
using MariamApp.DTOs.Suppliers;

namespace MariamApp.Mappings;

public class SuppliersMappingProfile : Profile
{
    public SuppliersMappingProfile()
    {
        CreateMap<SupplierRequest, Supplier>();
    }
}