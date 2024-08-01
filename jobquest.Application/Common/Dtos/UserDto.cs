namespace jobquest.Application.Common.Dtos;

public record UserDto(
    string? ID, 
    string? FirstName, 
    string? LastName, 
    string? Phone, 
    string? Email, 
    string? Password);