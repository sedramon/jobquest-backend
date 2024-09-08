using AutoMapper;
using jobquest.Application.Common.Dtos;
using jobquest.Domain.Entities;

namespace jobquest.Application.Mappers.Users;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserDto, User>()
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone ?? string.Empty))
            .ReverseMap();
        
        CreateMap<User, UserDisplayDto>().ReverseMap(); 
    }
}