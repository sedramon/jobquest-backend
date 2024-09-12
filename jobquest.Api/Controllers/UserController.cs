using jobquest.Application.Commands.Users;
using jobquest.Application.Common.Dtos;
using jobquest.Application.Exceptions;
using jobquest.Application.Queries.Users;
using jobquest.Domain.Entities;
using jobquest.Infrastructure.Services.Files;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace jobquest_backend.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ApplicationController
{
    private readonly ILogger<UserController> _logger;
    private readonly FileService _fileService;


    public UserController(ILogger<UserController> logger, FileService fileService)
    {
        _logger = logger;
        _fileService = fileService;
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
    
    [HttpPost("upload-profile-picture")]
    public async Task<IActionResult> UploadProfilePicture(IFormFile file, string userId)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        try
        {
            using (var fileStream = file.OpenReadStream())
            {
                var fileId = await _fileService.UploadFileAsync(fileStream, file.FileName, file.ContentType, userId);

                // Update the user with the profile picture ID
                var user = await DB.Find<User>().OneAsync(userId);
                if (user == null)
                    return NotFound("User not found.");

                user.ProfilePictureId = fileId;
                await user.SaveAsync();

                return Ok(new { fileId });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while uploading the profile picture.");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
        }
    }
    
    [HttpGet("get-profile-picture/{userId}")]
    public async Task<IActionResult> GetProfilePicture(string userId)
    {
        try
        {
            var user = await DB.Find<User>().OneAsync(userId);
            if (user == null || user.ProfilePictureId == null)
                return NotFound("Profile picture not found.");

            var fileData = await _fileService.DownloadFileAsync(user.ProfilePictureId.Value);
            var fileInfo = await _fileService.GetFileMetadataAsync(user.ProfilePictureId.Value);

            return File(fileData, fileInfo.Metadata["contentType"].AsString);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving the profile picture.");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
        }
    }
    
    
}