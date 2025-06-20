namespace Operators.Application.Exceptions;

public class ClientErrorException : Exception
{
    public ClientErrorException(string? message) : base(message)
    {
    }
}