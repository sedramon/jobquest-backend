namespace jobquest.Application.Common.Dtos;

public record UserDtoWithToken(UserDto User, string Token);