namespace Operators.Application;

public class ClientErrorException : Exception
{
    public ClientErrorException(string? message) : base(message)
    {
    }
}