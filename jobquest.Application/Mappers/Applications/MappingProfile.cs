using AutoMapper;
using jobquest.Application.Common.Dtos;
using jobquest.Domain.Entities;
using MongoDB.Entities;

namespace jobquest.Application.Mappers.Applications;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Domain.Entities.Application, ApplicationDto>()
            .ForMember(dest => dest.JobPost, opt => opt.MapFrom(src => src.JobPost))
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
            .ReverseMap();

        CreateMap<JobPost, JobPostDto>()
            .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company))
            .ReverseMap();

        CreateMap<One<Company>, CompanyDto>()
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ReverseMap();
    
        CreateMap<JobPost, Company>()
            .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.Company));

        CreateMap<User, UserDto>()
            .ReverseMap();
    }
}