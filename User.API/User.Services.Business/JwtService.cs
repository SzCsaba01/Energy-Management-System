using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Project.Data.Data.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using User.Services.Contracts;

namespace User.Services.Business;
public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> GetAuthentificationJwtAsync(UserEntity user)
    {
        var claimsIdentity = new ClaimsIdentity(new[] {
            new Claim("id", user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role) });
        var expires = DateTime.UtcNow.AddHours(6);

        var claims = claimsIdentity;
        var expirationDate = expires;

        return await GenerateJwtTokenAsync(claims, expirationDate);
    }

    private async Task<string> GenerateJwtTokenAsync(ClaimsIdentity claims, DateTime expirationDate)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration.GetSection("Jwt:Key").Value);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            Expires = expirationDate,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
