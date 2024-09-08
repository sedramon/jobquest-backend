using Amazon.Runtime.Internal;
using AutoMapper;
using jobquest.Application.Common.Dtos;
using jobquest.Application.Exceptions;
using MediatR;
using MongoDB.Driver;
using MongoDB.Entities;

namespace jobquest.Application.Queries.Applications;

public record GetAllByJobPostIdQuery(string JobPostId) : IRequest<List<ApplicationDto>>;


  public class GetAllByJobPostIdHandler : IRequestHandler<GetAllByJobPostIdQuery, List<ApplicationDto>>
    {
        private readonly IMapper _mapper;

        public GetAllByJobPostIdHandler(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<List<ApplicationDto>> Handle(GetAllByJobPostIdQuery request, CancellationToken cancellationToken)
{
    // Retrieve applications that match the given JobPostId
    var applicationEntities = await DB.Fluent<Domain.Entities.Application>()
        .Match(a => a.JobPost.ID == request.JobPostId)
        .ToListAsync(cancellationToken: cancellationToken);

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
                    ? _mapper.Map<CompanyDto>(await jobPost.Company.ToEntityAsync(cancellation: cancellationToken)) // Map Company to CompanyDto
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