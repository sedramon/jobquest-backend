using jobquest.Application.Commands.Users;
using jobquest.Application.Common.Dtos;
using jobquest.Application.Exceptions;
using jobquest.Application.Queries.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace jobquest_backend.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ApplicationController
{
    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger)
    {
        _logger = logger;
    }
    
    [HttpGet("get/all")]
    public async Task<OkObjectResult> GetAll() => Ok(await Mediator.Send(new GetAllQuery()));
    
    [HttpPost("create")]
    public async Task<OkObjectResult> CreateUser(UserDto dto) => Ok(await Mediator.Send(new CreateUserCommand(dto)));

    [HttpGet("get/one")]
    public async Task<IActionResult> GetOne(string email, string password)
    {
        try
        {
            return Ok(await Mediator.Send(new GetUserByEmailAndPasswordQuery(email, password)));
        }
        catch (UserNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing the request.");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
        }
        
    }
    
    [HttpPut("update")]
    public async Task<IActionResult> UpdateUser(UserDto dto)
    {
        try
        {
            return Ok(await Mediator.Send(new UpdateUserCommand(dto)));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
        }
    }
}