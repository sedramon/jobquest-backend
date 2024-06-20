namespace jobquest.Application.Common.Dtos;

public record JobPostDto(
    string? ID,
    string? Title,
    CompanyDto? Company,
    string? Description,
    string? FieldOfWork,
    string? Location,
    DateTime? EndsAt
    );