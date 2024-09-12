using AutoMapper;
using jobquest.Application.Common.Dtos;
using MediatR;
using MongoDB.Driver;
using MongoDB.Entities;

namespace jobquest.Application.Queries.JobPosts;

public record GetAllByCompanyIdQuery(string CompanyId) : IRequest<List<JobPostDto>>;

public class GetAllByCompanyIdHandler : IRequestHandler<GetAllByCompanyIdQuery, List<JobPostDto>>
{
    private readonly IMapper _mapper;

    public GetAllByCompanyIdHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<List<JobPostDto>> Handle(GetAllByCompanyIdQuery request, CancellationToken cancellationToken)
    {
        // Retrieve job posts by company ID
        var jobPostEntities = await DB.Fluent<Domain.Entities.JobPost>()
            .Match(j => j.Company.ID == request.CompanyId)
            .ToListAsync(cancellationToken: cancellationToken);

        var jobPostDtos = new List<JobPostDto>();

        // Map each JobPost entity to JobPostDto
        foreach (var jobPost in jobPostEntities)
        {
            // Fetch the Company entity related to this job post
            var company = jobPost.Company != null
                ? await jobPost.Company.ToEntityAsync(cancellation: cancellationToken)
                : null;

            // Map Company to CompanyDto
            var companyDto = company != null
                ? _mapper.Map<CompanyDto>(company)
                : null;

            // Create and add the JobPostDto to the list
            var jobPostDto = new JobPostDto(
                jobPost.ID,
                jobPost.Title,
                companyDto,  // Map the company details
                jobPost.Description,
                jobPost.FieldOfWork,
                jobPost.Location,
                jobPost.EndsAt,
                jobPost.CreatedOn
            );

            jobPostDtos.Add(jobPostDto);
        }

        return jobPostDtos;
    }
}
