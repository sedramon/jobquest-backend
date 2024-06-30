namespace jobquest.Application.Exceptions;

public class CompanyNotFoundException : Exception
{
    public CompanyNotFoundException(string email)
        : base($"Company with Email '{email}' does not exist.")
    {
    }
}