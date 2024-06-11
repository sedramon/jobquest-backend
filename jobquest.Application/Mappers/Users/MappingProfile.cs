using AutoMapper;
using jobquest.Application.Common.Dtos;
using jobquest.Domain.Entities;
using MongoDB.Entities;

namespace jobquest.Application.Mappers.Users;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserDto, User>().ReverseMap();
    }
}