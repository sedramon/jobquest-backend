﻿using jobquest.Application.Common.Dtos;
using jobquest.Application.Exceptions;
using jobquest.Domain.Entities;
using MediatR;
using MongoDB.Entities;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Driver;

namespace jobquest.Application.Queries.Applications;
    
public record GetAllByUserIdQuery(string UserId) : IRequest<List<ApplicationDto>>;

    public class GetAllByUserIdHandler : IRequestHandler<GetAllByUserIdQuery, List<ApplicationDto>>
    {
        private readonly IMapper _mapper;

        public GetAllByUserIdHandler(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<List<ApplicationDto>> Handle(GetAllByUserIdQuery request, CancellationToken cancellationToken)
        {
            // Retrieve all applications that match the given UserId
            var applicationEntities = await DB.Fluent<Domain.Entities.Application>()
                .Match(a => a.User.ID == request.UserId)
                .ToListAsync(cancellationToken: cancellationToken);
            var applicationDtos = new List<ApplicationDto>();

            // Manually map each Application entity to ApplicationDto
            foreach (var application in applicationEntities)
            {
                // Fetch the JobPost entity related to this application
                var jobPost = application.JobPost != null
                    ? await application.JobPost.ToEntityAsync(cancellation: cancellationToken)
                    : null;

                // Map JobPost to JobPostDto, including Company mapping
                var jobPostDto = jobPost != null
                    ? new JobPostDto(
                        jobPost.ID,
                        jobPost.Title,
                        jobPost.Company != null
                            ? _mapper.Map<CompanyDto>(await jobPost.Company.ToEntityAsync(cancellation: cancellationToken)) // Explicitly map Company to CompanyDto
                            : null,
                        jobPost.Description,
                        jobPost.FieldOfWork,
                        jobPost.Location,
                        jobPost.EndsAt,
                        jobPost.CreatedOn
                    )
                    : null;

                // Fetch the User entity related to this application (same as the UserId we're querying for)
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
