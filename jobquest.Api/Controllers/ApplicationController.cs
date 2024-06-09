using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace jobquest_backend.Controllers;

public class ApplicationController : ControllerBase
{
    private ISender? _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}