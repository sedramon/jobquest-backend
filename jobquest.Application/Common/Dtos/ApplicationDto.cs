namespace jobquest.Application.Common.Dtos;

public record ApplicationDto(string? ID, JobPostDto? JobPost, UserDto? User, DateTime? CreatedOn, DateTime? ModifiedOn);