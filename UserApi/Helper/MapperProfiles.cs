using AutoMapper;
using UserApi.Dtos;
using UserApi.models;

namespace UserApi.Helper
{
    public class MapperProfiles : Profile
    {

        public MapperProfiles()
        {
            CreateMap<UserModel, UserDto>();

            CreateMap<UserDto, UserModel>();

            CreateMap<UserModel, UserForCreationDto>();

            CreateMap<UserForCreationDto, UserModel>();
        }
    }
}
