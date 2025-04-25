using Microsoft.AspNetCore.Mvc;
using VolunteerMatch.Data.Repositories.Interfaces;
using VolunteerMatch.Models;

namespace VolunteerMatch.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrganizationFollowerController : ControllerBase
{
  private readonly IOrganizationFollowerRepository _organizationFollowerRepository;

  public OrganizationFollowerController(IOrganizationFollowerRepository organizationFollowerRepository)
  {
    _organizationFollowerRepository = organizationFollowerRepository;
  }

  [HttpGet("organizations/{organizationId:int}/followers")]
  public async Task<ActionResult<List<OrganizationFollower>>> GetFollowersByOrganizationIdAsync(int organizationId)
  {
    var followers = await _organizationFollowerRepository.GetFollowersByOrganizationIdAsync(organizationId);
    return Ok(followers);
  }
}