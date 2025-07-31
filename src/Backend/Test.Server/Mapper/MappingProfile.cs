using AutoMapper;
using Test.Server.DTOs;
using Test.Server.Models;

namespace Test.Server.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductResponseDto>();
        CreateMap<PriceDetail, PriceDetailResponseDto>();
    }
}
