using AutoMapper;
using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.ProductService.Dtos
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDetailsDto>()
                .ForMember(dest => dest.BrandName, option => option.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.TypeName, option => option.MapFrom(src => src.Type.Name))
                .ForMember(dest => dest.PictureUrl, option => option.MapFrom<ProductUrlResolver>());

            CreateMap<ProductBrand, BrandTypeDetailsDto>();
            CreateMap<ProductType, BrandTypeDetailsDto>();
        }
    }
}
