namespace jobquest.Application.Common.Dtos;

public record UserDisplayDto(
    string? ID, 
    string? FirstName, 
    string? LastName, 
    string? Phone, 
    string? Address,
    string? DateOfBirth,
    string? Interest,
    string? Email); 