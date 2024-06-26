﻿using AutoMapper;
using jobquest.Application.Common.Dtos;
using jobquest.Domain.Entities;

namespace jobquest.Application.Mappers.JobPosts;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<JobPost, JobPostDto>().ReverseMap();
        CreateMap<Company, CompanyDto>()
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ReverseMap();
        CreateMap<JobPost, Company>()
            .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.Company));
    }
}