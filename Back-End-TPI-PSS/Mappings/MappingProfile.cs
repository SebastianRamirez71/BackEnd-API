using AutoMapper;
using Back_End_TPI_PSS.Data.Entities;
using Back_End_TPI_PSS.Data.Models.ProductDTOs;
using Back_End_TPI_PSS.Data.Models.UserDTOs;

namespace Back_End_TPI_PSS.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserReturnDto>();

            CreateMap<Colour, ReturnColourDto>().ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.ColourName));
            CreateMap<Size, ReturnSizeDto>().ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.SizeName));
            CreateMap<Category, ReturnCategoryDto>().ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.CategoryName));
        }

    }
}
