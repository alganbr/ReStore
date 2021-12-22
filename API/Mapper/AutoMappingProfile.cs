using API.Entities;
using API.Models.Basket;
using API.Models.Product;
using AutoMapper;

namespace API.Mapper;

public class AutoMappingProfile : Profile
{
    public AutoMappingProfile()
    {
        CreateMap<Basket, BasketRs>();
        CreateMap<BasketItem, BasketItemRs>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
            .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => src.Product.PictureUrl))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Product.Type))
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Product.Brand));

        CreateMap<ProductRq, Product>();
        CreateMap<Product, ProductRs>();
    }
}