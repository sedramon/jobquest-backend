using jobquest.Application.Commands.Companies;
using jobquest.Application.Common.Dtos;
using jobquest.Application.Queries.Companies;
using Microsoft.AspNetCore.Mvc;

namespace jobquest_backend.Controllers;

[ApiController]
[Route("api/company")]
public class CompanyController : ApplicationController
{
    [HttpGet("get/all")]
    public async Task<OkObjectResult> GetAll() => Ok(await Mediator.Send(new GetAllQuery()));
    
    [HttpPost("create")]
    public async Task<OkObjectResult> CreateCompany(CompanyDto dto) => Ok(await Mediator.Send(new CreateCompanyCommand(dto)));

}