using jobquest.Application.Commands.Users;
using jobquest.Application.Common.Dtos;
using jobquest.Application.Exceptions;
using jobquest.Application.Queries.Users;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace jobquest_backend.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ApplicationController
{
    [HttpGet("get/all")]
    public async Task<OkObjectResult> GetAll() => Ok(await Mediator.Send(new GetAllQuery()));
    
    [HttpPost("create")]
    public async Task<OkObjectResult> CreateUser(UserDto dto) => Ok(await Mediator.Send(new CreateUserCommand(dto)));

    [HttpGet("get/one")]
    public async Task<IActionResult> GetOne(string email)
    {
        try
        {
            return Ok(await Mediator.Send(new GetUserByEmailQuery(email)));
        }
        catch (UserNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
        }
        
    }
}