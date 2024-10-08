﻿using jobquest.Application.Commands.Companies;
using jobquest.Application.Common.Dtos;
using jobquest.Application.Exceptions;
using jobquest.Application.Queries.Companies;
using jobquest.Application.Queries.JobPosts;
using Microsoft.AspNetCore.Mvc;
using GetAllQuery = jobquest.Application.Queries.Companies.GetAllQuery;

namespace jobquest_backend.Controllers;

[ApiController]
[Route("api/company")]
public class CompanyController : ApplicationController
{
    [HttpGet("get/all")]
    public async Task<OkObjectResult> GetAll() => Ok(await Mediator.Send(new GetAllQuery()));
    
    [HttpPost("create")]
    public async Task<OkObjectResult> CreateCompany(CompanyDto dto) => Ok(await Mediator.Send(new CreateCompanyCommand(dto)));

    [HttpGet("get/one")]
    public async Task<IActionResult> GetOne(string email, string password)
    {
        try
        {
            return Ok(await Mediator.Send(new GetCompanyByEmailAndPasswordQuery(email, password)));
        }
        catch (CompanyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
        }
    }
    
    [HttpPut("update")]
    public async Task<IActionResult> UpdateCompany(CompanyDto dto)
    {
        try
        {
            return Ok(await Mediator.Send(new UpdateCompanyCommand(dto)));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
        }
    }
    
    
}