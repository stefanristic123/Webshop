using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile   
    {

        public AutoMapperProfiles(){
            CreateMap<AppUser, MemberDto>()
                .ForMember(dest => dest.PhotoUrl, opt =>
                    opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDto>();
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.ProductPhotoUrl, opt =>
                    opt.MapFrom(src => src.ProductPhotos.FirstOrDefault(x => x.IsMain).Url));
            CreateMap<ProductPhoto, PhotoDto>();
            CreateMap<Cart, CartDto>();
            CreateMap<CartItem, CartItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.ProductPhotoUrl, opt => opt.MapFrom(src => src.Product.ProductPhotos.FirstOrDefault(p => p.IsMain).Url));
            CreateMap<Order, OrderDto>();
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.ProductPhotoUrl, opt => opt.MapFrom(src => src.Product.ProductPhotos.FirstOrDefault(p => p.IsMain).Url));
            CreateMap<MemberUpdateDto, AppUser>();
        }
        
    }
}