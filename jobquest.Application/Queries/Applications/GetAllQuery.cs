using AutoMapper;
using jobquest.Application.Common.Dtos;
using MediatR;
using MongoDB.Driver;
using MongoDB.Entities;

namespace jobquest.Application.Queries.Applications;

public record GetAllQuery : IRequest<List<ApplicationDto>>;

public class GetAllHandler : IRequestHandler<GetAllQuery, List<ApplicationDto>>
{
    private readonly IMapper _mapper;

    public GetAllHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<List<ApplicationDto>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        var applicationEntities =
            await DB.Fluent<Domain.Entities.Application>().ToListAsync(cancellationToken: cancellationToken);

        var applicationDtos = new List<ApplicationDto>();

        foreach (Domain.Entities.Application application in applicationEntities)
        {
            var jobPost = application.JobPost != null
                ? await application.JobPost.ToEntityAsync(cancellation: cancellationToken)
                : null;

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

            var user = application.User != null
                ? await application.User.ToEntityAsync(cancellation: cancellationToken)
                : null;

            var userDto = user != null
                ? new UserDto(
                    user.ID,
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.Phone,
                    null
                )
                : null;

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
