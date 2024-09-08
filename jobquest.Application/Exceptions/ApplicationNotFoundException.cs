namespace jobquest.Application.Exceptions;

public class ApplicationNotFoundException : Exception
{
    public ApplicationNotFoundException(string id) : base($"""Application with ID '{id}' does not exist.""")
    {
    }
}