namespace jobquest.Application.Commands.Authentication;

public interface ITokenService
{
    string GenerateToken(string email);
}