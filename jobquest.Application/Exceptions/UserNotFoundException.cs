namespace jobquest.Application.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(string email)
        : base($"User with Email '{email}' does not exist.")
    {
    }
}