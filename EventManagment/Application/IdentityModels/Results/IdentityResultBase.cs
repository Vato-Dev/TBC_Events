namespace Application.IdentityModels.Results;

public abstract class IdentityResultBase<TInheritorResult> where TInheritorResult : IdentityResultBase<TInheritorResult>, new()
{
    public bool Succeeded { get; set; }
    public IEnumerable<ApplicationError>? Errors { get; set; }
    public TokensReponse? Tokens { get; set; }

    public static TInheritorResult Succeed(TokensReponse tokensReponse) => 
        new() { Succeeded = true, Tokens = tokensReponse };
    
    public static TInheritorResult Failed(IEnumerable<ApplicationError> errors)
        => new() { Succeeded = false, Errors = errors.ToArray() };

}

public sealed class ApplicationError(string Code, string Description)
{
    public string Code {get; set;} = Code;
    public string Description {get; set;} =  Description;
};

public class TokensReponse
{
    public string AccessToken { get; set; } = string.Empty;
  //  public string RefreshToken { get; set; }
}