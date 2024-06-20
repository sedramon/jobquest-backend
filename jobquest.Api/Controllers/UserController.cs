using jobquest.Application.Commands.Users;
using jobquest.Application.Common.Dtos;
using jobquest.Application.Queries.Users;
using Microsoft.AspNetCore.Mvc;

namespace jobquest_backend.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ApplicationController
{
    [HttpGet("get/all")]
    public async Task<OkObjectResult> GetAll() => Ok(await Mediator.Send(new GetAllQuery()));
    
    [HttpPost("create")]
    public async Task<OkObjectResult> CreateUser(UserDto dto) => Ok(await Mediator.Send(new CreateUserCommand(dto)));
}