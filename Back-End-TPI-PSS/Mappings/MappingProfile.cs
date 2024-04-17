using AutoMapper;
using Back_End_TPI_PSS.Data.Entities;
using Back_End_TPI_PSS.Data.Models.UserDTOs;

namespace Back_End_TPI_PSS.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
       
            CreateMap<User, UserReturnDto>();

        }

    }
}
