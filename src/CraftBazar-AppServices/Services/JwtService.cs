// Responsible for: Generate JWT Token, Read Claims & Generate Security Token
using Microsoft.Extensions.Options;
using CraftBazar_DTO.Settings;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

public class JwtService : IJwtService
{
    private readonly JwtSettings _jwtSettings;

    public JwtService(
        IOptions<JwtSettings> jwtOptions)
    {
        _jwtSettings = jwtOptions.Value;
    }
    public string GenerateToken(User user)
    {
        var claims = new List<Claim>     // These are pieces of information stored in the token.
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.GivenName, user.FirstName),
            new Claim(ClaimTypes.Role, user.Role.Name)
        };

        var key = new SymmetricSecurityKey(                 // Convert secret key to bytes
             Encoding.UTF8.GetBytes(_jwtSettings.Key)
         );

        var credentials = new SigningCredentials(           // Tell JWT which algorithm to use
            key,
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
