using jobquest.Application.Commands.JobPosts;
using jobquest.Application.Common.Dtos;
using jobquest.Application.Queries.JobPosts;
using Microsoft.AspNetCore.Mvc;

namespace jobquest_backend.Controllers;

[ApiController]
[Route("api/jobpost")]
public class JobPostController : ApplicationController
{
    [HttpPost("create")]
    public async Task<OkObjectResult> CreateJobPost(JobPostDto dto) => Ok(await Mediator.Send(new CreateJobPostCommand(dto)));
    
    [HttpGet("get/all")]
    public async Task<OkObjectResult> GetAll() => Ok(await Mediator.Send(new GetAllQuery()));
    
    [HttpDelete("delete")]
    public async Task<OkObjectResult> DeleteJobPost(string jobPostId) =>
        Ok(await Mediator.Send(new DeleteJobPostCommand(jobPostId)));
    
    [HttpGet("get/one")]
    public async Task<OkObjectResult> GetOne(string jobPostId) => Ok(await Mediator.Send(new GetOneQuery(jobPostId)));
}