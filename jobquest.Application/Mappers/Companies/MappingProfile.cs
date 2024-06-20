using AutoMapper;
using jobquest.Application.Common.Dtos;
using jobquest.Domain.Entities;
using MongoDB.Entities;

namespace jobquest.Application.Mappers.Companies;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CompanyDto, Company>().ReverseMap();
    }
}