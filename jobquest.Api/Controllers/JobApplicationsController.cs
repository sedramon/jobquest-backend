using jobquest.Application.Commands.Applications;
using jobquest.Application.Common.Dtos;
using jobquest.Application.Exceptions;
using jobquest.Application.Queries.Applications;
using Microsoft.AspNetCore.Mvc;

namespace jobquest_backend.Controllers;

[ApiController]
[Route("api/application")]
public class JobApplicationsController : ApplicationController
{
    [HttpPost("create")]
    public async Task<OkObjectResult> CreateApplication(ApplicationDto dto) => Ok(await Mediator.Send(new CreateApplicationCommand(dto)));

    [HttpGet("get/all")]
    public async Task<OkObjectResult> GetAll() => Ok(await Mediator.Send(new GetAllQuery())); 

    [HttpGet("{jobPostId}/applications/jobpost")]
    public async Task<IActionResult> GetApplicationsByJobPostId(string jobPostId)
    {
        try
        {
            var applications = await Mediator.Send(new GetAllByJobPostIdQuery(jobPostId));
            return Ok(applications);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); // Return detailed error message
        }
    }
    
    [HttpGet("{userId}/applications/user")]
    public async Task<IActionResult> GetApplicationsByUserId(string userId)
    {
        try
        {
            var applications = await Mediator.Send(new GetAllByUserIdQuery(userId));
            return Ok(applications);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); // Return detailed error message
        }
    }
    
    [HttpPost("jobposts/applications")]
    public async Task<IActionResult> GetApplicationsByJobPostIds([FromBody] List<string> jobPostIds)
    {
        try
        {
            var applications = await Mediator.Send(new GetAllByJobPostIdsQuery(jobPostIds));
            return Ok(applications);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

}