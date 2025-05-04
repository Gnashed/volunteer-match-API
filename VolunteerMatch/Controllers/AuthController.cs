using Microsoft.AspNetCore.Mvc;
using VolunteerMatch.Helpers;
using VolunteerMatch.Models.Requests;

namespace VolunteerMatch.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController: ControllerBase
{
  private readonly IConfiguration _config;

  public AuthController(IConfiguration config)
  {
    _config = config;
  }

  [HttpPost("login")]
  public IActionResult Login([FromBody] CreateLoginRequest loginRequest)
  {
    if (loginRequest.Email == "testuser@email.com" && loginRequest.Password == "password")
    {
      var token = JwtTokenGenerator.GenerateJwtToken(loginRequest.Email, _config);
      return Ok(new { token });
    }
    
    return Unauthorized();
  }
}