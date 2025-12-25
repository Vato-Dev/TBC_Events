using Application.Models;
using Domain.Models;

namespace Application.IdentityModels.Results;

public class AuthResult : IdentityResultBase<AuthResult>
{  
    public int UserId { get; set; }
    public string? UserName { get; set; }
    public UserRole Role { get; set; }
    
    public static AuthResult Succeed(TokensReponse tokensReponse,int id, UserRole role) => 
        new() { Succeeded = true, Tokens = tokensReponse , UserId  = id , Role = role };
    public static AuthResult Succeed(TokensReponse tokensReponse,int id, string userName) => 
        new() { Succeeded = true, Tokens = tokensReponse , UserId  = id ,UserName =  userName };
}

public class LoginResponse
{
    public int UserId { get; set; } 
    public string UserName { get; set; } 
    public string AccessToken { get; set; } 

    public static LoginResponse Response(AuthResult authResult)
    {
        return new LoginResponse { UserId = authResult.UserId, UserName = authResult.UserName! , AccessToken = authResult.Tokens!.AccessToken };
    }

}