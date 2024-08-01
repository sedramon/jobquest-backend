using AutoMapper;
using jobquest.Application.Common.Dtos;
using jobquest.Domain.Entities;
using MediatR;
using MongoDB.Driver;
using MongoDB.Entities;

namespace jobquest.Application.Queries.JobPosts;

public record GetOneQuery(string JobPostId) : IRequest<JobPostDto>;

public class GetOneHandler : IRequestHandler<GetOneQuery, JobPostDto>
{
    private readonly IMapper _mapper;

    public GetOneHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<JobPostDto> Handle(GetOneQuery request, CancellationToken cancellationToken)
    {
        var jobPost = await DB.Fluent<JobPost>()
            .Match(jobPost => jobPost.ID == request.JobPostId)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        var jobPostDto = new JobPostDto(
            jobPost.ID,
            jobPost.Title,
            _mapper.Map<CompanyDto>(await jobPost.Company.ToEntityAsync(cancellation: cancellationToken)),
            jobPost.Description,
            jobPost.FieldOfWork,
            jobPost.Location,
            jobPost.EndsAt,
            jobPost.CreatedOn
        );
        return jobPostDto;
    }
}