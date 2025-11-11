using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using webProductos.Application.Contracts;
using webProductos.Domain.Entities;

namespace webProductos.Infrastructure.Security;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration _configuration;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        var jwtKey = _configuration["Jwt:Key"];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var claims = new List<Claim>
        {
            
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            
            new Claim(ClaimTypes.Role, user.Role)
        };

        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            Expires = DateTime.UtcNow.AddHours(8),
            SigningCredentials = creds
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
            
        return tokenHandler.WriteToken(token);
    }
}