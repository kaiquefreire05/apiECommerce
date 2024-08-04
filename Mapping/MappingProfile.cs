using AutoMapper;
using ECommerceApi.DTOs;
using ECommerceApi.Models;

namespace ECommerceApi.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map between OrderDTO and OrderModel
            CreateMap<OrderDTO, OrderModel>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrdersItemDTO));
            CreateMap<OrderModel, OrderDTO>()
                .ForMember(dest => dest.OrdersItemDTO, opt => opt.MapFrom(src => src.OrderItems));

            // Map between OrderItemDTO and OrderItemModel
            CreateMap<OrderItemDTO, OrderItemModel>();
            CreateMap<OrderItemModel, OrderItemDTO>();

            // Map between ProductDTO and Product
            CreateMap<ProductDTO, ProductModel>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrdersItemDTO));
            CreateMap<ProductModel, ProductDTO>()
                .ForMember(dest => dest.OrdersItemDTO, opt => opt.MapFrom(src => src.OrderItems));

            // Map between UserDTO and User
            CreateMap<UserDTO, UserModel>();
            CreateMap<UserModel, UserDTO>();
        }
    }
}
