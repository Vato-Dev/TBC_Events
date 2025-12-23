using Application.Models;
using Domain.Models;

namespace Application.IdentityModels.Results;

public class AuthResult : IdentityResultBase<AuthResult>
{  
    public int UserId { get; set; }
    public UserRole Role { get; set; }
    
    public static AuthResult Succeed(TokensReponse tokensReponse,int id, UserRole role) => 
        new() { Succeeded = true, Tokens = tokensReponse , UserId  = id , Role = role };
}