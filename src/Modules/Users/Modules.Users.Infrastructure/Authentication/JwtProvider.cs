using Modules.Users.Domain;
using Modules.Users.Application.Abstractions;
using Modules.Common.Infrastructure.Authentication;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
namespace Modules.Users.Infrastructure.Authentication;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _jwtOptions;
    public JwtProvider(IOptions<JwtOptions> options)
    {
        _jwtOptions = options.Value;
    }
    public string GenerateAccesss(User user)
    {
        var claims = new Claim[]
        {
            new(CustomClaims.Sub, user.id.ToString()),
            new(CustomClaims.Email, user.Email),
            new(CustomClaims.Role, user.Role),
        };

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.LifeTimeInMinutes),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateReferesh()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }

}
