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
        var Claims = new Claim[] {
         new(CustomClaims.Sub,user.id.ToString()) ,
         new(CustomClaims.Email ,user.Email ),
        //  new(CustomClaims.Role, user.Role )
     };

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            Claims,
            null,
            DateTime.UtcNow.AddMinutes(_jwtOptions.LifeTimeInMinutes));

        string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue;
    }

    public string GenerateReferesh()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }

}
