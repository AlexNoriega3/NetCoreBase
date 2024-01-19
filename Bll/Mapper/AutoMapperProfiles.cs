using AutoMapper;
using Models.DTOs;
using Models.DTOs.user;
using Models.Entities;

namespace Bll.Mapper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<baseEntity, baseEntityDto>().ReverseMap();

            CreateMap<ApiUserDto, AppUsuario>()
                .ReverseMap();

            CreateMap<AppUserDto, AppUsuario>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<UserCreateEditDto, AppUsuario>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.Image, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<UserPostDto, AppUsuario>()
                .ReverseMap();

            CreateMap<UserPutDto, AppUsuario>()
                .ForMember(x => x.Image, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<RoleDto, Role>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}