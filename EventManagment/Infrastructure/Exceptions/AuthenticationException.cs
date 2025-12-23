namespace Infrastructure.Exceptions;

public class AuthenticationException : Exception
{
    public AuthenticationException() : base("Invalid Email/UserName or Password")
    {
        
    }
    public AuthenticationException(string message) : base(message)
    {
        
    }
    public AuthenticationException(string message, Exception inner) : base(message, inner)
    {
    }
}