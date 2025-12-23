namespace Application.Exceptions;

public class ForbiddenAccessException : Exception
{

    public ForbiddenAccessException() : base("Forbidden access exception")
    {
        
    }

    public ForbiddenAccessException(string message) : base(message)
    {
        
    }
    public ForbiddenAccessException( string message, Exception innerException) : base(message, innerException)
    {
    }
}