using AutoMapper;
using jobquest.Application.Common.Dtos;
using jobquest.Domain.Entities;
using MediatR;
using MongoDB.Driver;
using MongoDB.Entities;

namespace jobquest.Application.Commands.Applications;

public record CreateApplicationCommand(ApplicationDto ApplicationDto) : IRequest<ApplicationDto>;

public class CreateApplicationHandlers : IRequestHandler<CreateApplicationCommand, ApplicationDto>
{
    private readonly IMapper _mapper;
    
    public CreateApplicationHandlers(IMapper mapper)
    {
        _mapper = mapper;
    }
    
    public async Task<ApplicationDto> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
    {
        var user = await DB.Fluent<User>()
            .Match(user => user.ID == request.ApplicationDto.User.ID)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        var jobPost = await DB.Fluent<JobPost>()
            .Match(jp => jp.ID == request.ApplicationDto.JobPost.ID)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        
        var jobPostDto = _mapper.Map<JobPostDto>(jobPost);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        
        if (jobPost == null)
        {
            throw new Exception("Job post not found");
        }

        var application = new Domain.Entities.Application
        {
            CreatedOn = DateTime.UtcNow,
            ModifiedOn = DateTime.UtcNow
        };
        
        application.JobPost = new One<JobPost>(jobPost.ID);
        application.User = new One<User>(user.ID);

        var saveTask =  application.SaveAsync(cancellation: cancellationToken);
        await saveTask;

        if (saveTask.IsCompletedSuccessfully)
        {
            var applicationDto = new ApplicationDto(application.ID, jobPostDto, _mapper.Map<UserDisplayDto>(user), application.CreatedOn, application.ModifiedOn);

            return applicationDto;
        }
        else
        {
            throw new Exception("Failed to save the application");
        }
    }
}
