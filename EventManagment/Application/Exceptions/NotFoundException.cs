namespace Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException() : base("Not Found")
    {
        
    }
    public NotFoundException(string message, Exception innerException) : base(message, innerException) { }
}