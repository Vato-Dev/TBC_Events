namespace Application.IdentityModels.Results;

public class RegisterResult : IdentityResultBase<RegisterResult>
{
    public int? UserId { get; init; }

    public IdentityRegisterError? Error { get; init; }

    public static RegisterResult Success(int userId ,TokensReponse tokensReponse)
        => new() { Succeeded = true, UserId = userId , Tokens = tokensReponse };

    public static RegisterResult InvalidOtp()
        => new() { Succeeded = false, Error = IdentityRegisterError.InvalidOtp};
}

public enum IdentityRegisterError
{
    InvalidOtp
}