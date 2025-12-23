using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.models;

public class JwtServiceOptions
{
    public const string Authentication = "Authentication";

    public required string SecretKey { get; set; }
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public SecurityKey GetIssuerSigningKey() =>
        new SymmetricSecurityKey(Convert.FromBase64String(SecretKey));
    
}