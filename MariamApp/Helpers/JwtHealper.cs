using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MariamApp.JWT;

public class JwtGenerator
{
    private readonly string _secret;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expiryMinutes;

    public JwtGenerator(IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        _secret = jwtSettings["Secret"];
        _issuer = jwtSettings["Issuer"];
        _audience = jwtSettings["Audience"];
        _expiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"]);
    }

    public string GenerateToken(string username, string role, string userId)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(ClaimTypes.Role, role),
            new Claim(ClaimTypes.Name, userId),
        };
        
        var token = new JwtSecurityToken(
            _issuer,
            _audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(_expiryMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
