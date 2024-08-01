using AutoMapper;
using MediatR;
using MongoDB.Driver;
using MongoDB.Entities;
using jobquest.Application.Common.Dtos;
using jobquest.Domain.Entities;


namespace jobquest.Application.Commands.JobPosts;

public record CreateJobPostCommand(JobPostDto JobPostDto) : IRequest<JobPostDto>;

public class CreateJobPostHandlers : IRequestHandler<CreateJobPostCommand, JobPostDto>
{
    private readonly IMapper _mapper;
    
    public CreateJobPostHandlers(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<JobPostDto> Handle(CreateJobPostCommand request, CancellationToken cancellationToken)
    {
        var company = new Company();
        if (request.JobPostDto.Company.CompanyName != "")
        {
            company = await DB.Fluent<Company>()
                .Match(company => company.CompanyName == request.JobPostDto.Company.CompanyName)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }

        var jobPost = new JobPost
        {
            Title = request.JobPostDto.Title,
            Description = request.JobPostDto.Description,
            FieldOfWork = request.JobPostDto.FieldOfWork,
            Location = request.JobPostDto.Location,
            EndsAt = request.JobPostDto.EndsAt.GetValueOrDefault(DateTime.UtcNow),
            CreatedOn = DateTime.UtcNow,
            ModifiedOn = DateTime.UtcNow
        };

        // Explicitly create an instance of One<Company> when assigning it to jobPost.Company
        jobPost.Company = company.ID != null ? new One<Company>(company.ID) : null;

        await jobPost.SaveAsync(cancellation: cancellationToken);

        var jobPostDto = new JobPostDto(
            jobPost.ID,
            jobPost.Title,
            company.ID != null ? _mapper.Map<CompanyDto>(company) : null,
            jobPost.Description,
            jobPost.FieldOfWork,
            jobPost.Location,
            jobPost.EndsAt,
            jobPost.CreatedOn
        );

        return jobPostDto;

    }

}