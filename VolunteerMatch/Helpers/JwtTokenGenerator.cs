using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace VolunteerMatch.Helpers;

public static class JwtTokenGenerator
{
  // Helper method that generates JWTs.
  public static string GenerateJwtToken(string username, IConfiguration config)
  {
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]!));
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var claims = new[]
    {
      new Claim(ClaimTypes.Name, username),
      new Claim(ClaimTypes.Role, "User")
    };

    var token = new JwtSecurityToken(
      issuer: config["Jwt:Issuer"],
      audience: config["Jwt:Audience"],
      claims: claims,
      expires: DateTime.UtcNow.AddMinutes(30),
      signingCredentials: credentials
    );
    
    return new JwtSecurityTokenHandler().WriteToken(token);
  }
}