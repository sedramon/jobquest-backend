using AutoMapper;
using jobquest.Application.Common.Dtos;
using jobquest.Application.Exceptions;
using MediatR;
using MongoDB.Driver;
using MongoDB.Entities;

namespace jobquest.Application.Queries.Applications;

public record GetAllByJobPostIdsQuery(List<string> JobPostIds) : IRequest<List<ApplicationDto>>;

public class GetAllByJobPostIdsHandler : IRequestHandler<GetAllByJobPostIdsQuery, List<ApplicationDto>>
{
    private readonly IMapper _mapper;

    public GetAllByJobPostIdsHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<List<ApplicationDto>> Handle(GetAllByJobPostIdsQuery request, CancellationToken cancellationToken)
    {
        // Match applications that have JobPost IDs in the provided array
        var applicationEntities = await DB.Fluent<Domain.Entities.Application>()
            .Match(a => request.JobPostIds.Contains(a.JobPost.ID)) // Match any of the job post IDs
            .ToListAsync(cancellationToken: cancellationToken);

        if (applicationEntities == null || !applicationEntities.Any())
        {
            throw new ApplicationNotFoundException("No applications found for the provided JobPost IDs.");
        }

        var applicationDtos = new List<ApplicationDto>();

        // Manually map each Application entity to ApplicationDto
        foreach (var application in applicationEntities)
        {
            // Fetch the JobPost entity related to this application
            var jobPost = application.JobPost != null
                ? await application.JobPost.ToEntityAsync(cancellation: cancellationToken)
                : null;

            // Map JobPost to JobPostDto
            var jobPostDto = jobPost != null
                ? new JobPostDto(
                    jobPost.ID,
                    jobPost.Title,
                    jobPost.Company != null
                        ? _mapper.Map<CompanyDto>(await jobPost.Company.ToEntityAsync(cancellation: cancellationToken))
                        : null,
                    jobPost.Description,
                    jobPost.FieldOfWork,
                    jobPost.Location,
                    jobPost.EndsAt,
                    jobPost.CreatedOn
                )
                : null;

            // Fetch the User entity related to this application
            var user = application.User != null
                ? await application.User.ToEntityAsync(cancellation: cancellationToken)
                : null;

            // Map User to UserDto
            var userDto = user != null
                ? new UserDisplayDto(
                    user.ID,
                    user.FirstName,
                    user.LastName,
                    user.Phone,
                    user.Address,
                    user.DateOfBirth,
                    user.Interest,
                    user.Email// Add additional user-related fields if necessary
                )
                : null;

            // Create and add the ApplicationDto to the list
            var applicationDto = new ApplicationDto(
                application.ID,
                jobPostDto,
                userDto,
                application.CreatedOn,
                application.ModifiedOn
            );

            applicationDtos.Add(applicationDto);
        }

        return applicationDtos;
    }
}