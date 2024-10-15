using AutoMapper;
using ProductApi.Data.Models;
using ProductApi.Models.DTO;
using ProductApi.Models.Requests;

namespace ProductApi.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<ProductVersion, ProductVersionDTO>().ReverseMap();

            CreateMap<ProductCreateRequest, ProductDTO>()
                .AfterMap((src, dest) => dest.ID = Guid.NewGuid());
            CreateMap<ProductUpdateRequest, ProductDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(product => product.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(product => product.Description))
                .ForMember(dest => dest.ID, opt => opt.UseDestinationValue());

            CreateMap<ProductVersionCreateRequest, ProductVersionDTO>()
                .AfterMap((src, dest) => dest.ID = Guid.NewGuid())
                .AfterMap((src, dest) => dest.CreatingDate = DateTime.UtcNow);
            CreateMap<ProductVersionUpdateRequest, ProductVersionDTO>()
                .ForMember(dest => dest.ProductID, opt => opt.MapFrom(product => product.ProductID))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(product => product.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(product => product.Description))
                .ForMember(dest => dest.Width, opt => opt.MapFrom(product => product.Width))
                .ForMember(dest => dest.Height, opt => opt.MapFrom(product => product.Height))
                .ForMember(dest => dest.Length, opt => opt.MapFrom(product => product.Length))
                .ForMember(dest => dest.CreatingDate, opt => opt.UseDestinationValue())
                .ForMember(dest => dest.ID, opt => opt.UseDestinationValue());
        }
    }
}
