using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace TechLibrary.Api.Infrastructure.Security.Token.Access;

public class TokenJwtGenerator
{
    public string GenerateToken(string email)
    {
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Expires = DateTime.UtcNow.AddHours(24 * 7), // 7 days
            SigningCredentials = new SigningCredentials(SecurityKey(), SecurityAlgorithms.HmacSha256Signature),
        };
        var tokenHandler = new JwtSecurityTokenHandler();

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);
    }

    private static SymmetricSecurityKey SecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mkj2h34h2342h3d23x43ioczcfdg347963453450df"));
    }
}
