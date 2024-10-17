using AutoMapper;
using ProductApplication.Models;

namespace ProductApplication.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductViewModel>().ReverseMap();
            CreateMap<Product, ProductVersionViewModel>().ReverseMap();
        }
    }
}
