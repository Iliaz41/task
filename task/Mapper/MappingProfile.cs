using AutoMapper;
using task.Dtos;
using task.Models;

namespace task.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()     
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, CreateUserDto>().ReverseMap();
            CreateMap<User, UpdateUserDto>().ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<Role, CreateRoleDto>().ReverseMap();
            CreateMap<CreateUser, CreateUserDto>().ReverseMap();
            CreateMap<Criteria, CriteriaDto>().ReverseMap();
            CreateMap<Auth, AuthDto>().ReverseMap();
        }
    }
}
