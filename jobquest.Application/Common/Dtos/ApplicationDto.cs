using MongoDB.Bson;

namespace jobquest.Application.Common.Dtos;

public record ApplicationDto(
    string? ID, 
    JobPostDto? JobPost, 
    UserDisplayDto? User,
    DateTime? CreatedOn, 
    DateTime? ModifiedOn);