using jobquest.Application.Queries.Users;
using Microsoft.AspNetCore.Mvc;

namespace jobquest_backend.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ApplicationController
{
    [HttpPost("get/all")]
    public async Task<OkObjectResult> GetAl() => Ok(await Mediator.Send(new GetAllQuery()));
}