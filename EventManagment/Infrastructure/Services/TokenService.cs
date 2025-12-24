using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Infrastructure.models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Persistence.IdentityModels;

namespace Infrastructure.Services;

public class TokenService(IOptions<JwtServiceOptions> options)
{

    public string GenerateAccessToken(ApplicationUser user , IList<string> roles)
    {
        var signInCredentials = new SigningCredentials(options.Value.GetIssuerSigningKey(), SecurityAlgorithms.HmacSha256);
        
        var claims = new List<Claim>
        {
            new ("Sid", user.Id.ToString()),
            new ("Preferred_name", user.UserName!)
        };
        
        foreach (var role in roles)
        {
            claims.Add(new Claim("Role", role));
        }

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: options.Value.Issuer,
            audience: options.Value.Audience,
            claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(4),
            signInCredentials);
        
        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }
}