using AutoMapper;
using jobquest.Application.Common.Dtos;
using jobquest.Domain.Entities;
using MediatR;
using MongoDB.Driver;
using MongoDB.Entities;

namespace jobquest.Application.Queries.JobPosts;

public record GetAllQuery : IRequest<List<JobPostDto>>;


public class GetAllHandler : IRequestHandler<GetAllQuery, List<JobPostDto>>
{
    private readonly IMapper _mapper;

    public GetAllHandler(IMapper mapper)
    {
        _mapper = mapper;
    }
    
    public async Task<List<JobPostDto>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        var jobPostEntities = await DB.Fluent<JobPost>().ToListAsync(cancellationToken: cancellationToken);

        var jobPostDtos = new List<JobPostDto>();

        foreach (JobPost jobPost in jobPostEntities)
        {
            var company = jobPost.Company != null ? await jobPost.Company.ToEntityAsync(cancellation: cancellationToken) : null;

            var companyDto = company != null ? new CompanyDto(
                company.ID,
                company.CompanyName,
                company.CompanyAddress,
                company.CompanyPhone,
                company.Activity,
                company.MB,
                company.PIB,
                company.Email,
                null // Exclude the Password field
            ) : null;
            
            var jobPostDto = new JobPostDto(
                jobPost.ID,
                jobPost.Title,
                companyDto,
                jobPost.Description,
                jobPost.FieldOfWork,
                jobPost.Location,
                jobPost.EndsAt
            );
            jobPostDtos.Add(jobPostDto);
        }

        return jobPostDtos;
    }
}