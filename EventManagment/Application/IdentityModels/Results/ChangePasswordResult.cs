namespace Application.IdentityModels.Results;

public class ChangePasswordResult :  IdentityResultBase<ChangePasswordResult>
{
    public int? UserId { get; init; }

    public IdentityChangePasswordError? Error { get; init; }
    
    public static ChangePasswordResult Success(int userId ,TokensReponse tokensReponse)
        => new() { Succeeded = true, UserId = userId , Tokens = tokensReponse };

    public static ChangePasswordResult InvalidPassword()
        => new() { Succeeded = false, Error = IdentityChangePasswordError.IncorrectPassword};
    
    public static ChangePasswordResult PasswordDoesNotMatch()
        => new() { Succeeded = false, Error = IdentityChangePasswordError.PasswordsDoesNotMatch};

}

public enum IdentityChangePasswordError
{
    IncorrectPassword,
    PasswordsDoesNotMatch
}