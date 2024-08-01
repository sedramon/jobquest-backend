using jobquest.Application.Commands.Applications;
using jobquest.Application.Common.Dtos;
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

}