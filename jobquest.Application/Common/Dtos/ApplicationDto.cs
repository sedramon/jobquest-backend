namespace jobquest.Application.Common.Dtos;

public record ApplicationDto(string? ID, CompanyDto? Company, UserDto? User, DateTime? CreatedOn, DateTime? ModifiedOn);