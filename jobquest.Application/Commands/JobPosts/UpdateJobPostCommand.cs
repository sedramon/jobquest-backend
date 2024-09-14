using AutoMapper;
using jobquest.Application.Common.Dtos;
using jobquest.Domain.Entities;
using MediatR;
using MongoDB.Driver;
using MongoDB.Entities;

namespace jobquest.Application.Commands.JobPosts;

public record UpdateJobPostCommand(JobPostDto JobPostDto) : IRequest<JobPostDto>;

public class UpdateJobPostHandler : IRequestHandler<UpdateJobPostCommand, JobPostDto>
{
    private readonly IMapper _mapper;

    public UpdateJobPostHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<JobPostDto> Handle(UpdateJobPostCommand request, CancellationToken cancellationToken)
    {
        // Retrieve the job post from the database using its ID
        var existingJobPost = await DB.Find<JobPost>().OneAsync(request.JobPostDto.ID);

        if (existingJobPost == null)
        {
            throw new Exception("JobPost not found");
        }

        // Map updated fields from the DTO to the existing job post
        _mapper.Map(request.JobPostDto, existingJobPost);

        // Update the modified date
        existingJobPost.ModifiedOn = DateTime.UtcNow;

        // If the company is being updated, retrieve or create the company
        if (request.JobPostDto.Company != null && !string.IsNullOrWhiteSpace(request.JobPostDto.Company.CompanyName))
        {
            var company = await DB.Fluent<Company>()
                .Match(c => c.CompanyName == request.JobPostDto.Company.CompanyName)
                .FirstOrDefaultAsync(cancellationToken);

            if (company == null)
            {
                // Handle the case where the company is not found or create a new one
                company = new Company { CompanyName = request.JobPostDto.Company.CompanyName };
                await company.SaveAsync(cancellation: cancellationToken);
            }

            // Associate the job post with the company
            existingJobPost.Company = new One<Company>(company.ID);
        }

        // Save the updated job post
        await existingJobPost.SaveAsync(cancellation: cancellationToken);

        // Return the updated job post DTO
        var updatedJobPostDto = _mapper.Map<JobPostDto>(existingJobPost);
        return updatedJobPostDto;
    }
}