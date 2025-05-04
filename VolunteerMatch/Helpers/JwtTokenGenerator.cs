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
    // Secret key from our user secrets.
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]!));
    // Using a security algorithm that is the standard for signing JWTs.
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    
    // Here we are defining the token's claim.
    // A claim is like a payload of the JWT. It's the actual data inside the token.
    var claims = new[]
    {
      // This stores the username
      new Claim(ClaimTypes.Name, username),
      // Store the user's role, "User" by default.
      new Claim(ClaimTypes.Role, "User")
    };
    
    // This token object will eventually be ready to be encoded into a string.
    var token = new JwtSecurityToken(
      issuer: config["Jwt:Issuer"], // The Web API.
      audience: config["Jwt:Audience"], // Frontend
      claims: claims, // username, role, etc.
      expires: DateTime.UtcNow.AddMinutes(30),
      signingCredentials: credentials // This let .NET know how to sign the token.
    );
    
    // Encoded string. It's what gets returned to the client and is used in every future request.
    return new JwtSecurityTokenHandler().WriteToken(token);
  }
}