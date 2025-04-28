using Microsoft.AspNetCore.Mvc;
using VolunteerMatch.Data.Repositories.Interfaces;
using VolunteerMatch.Models;

namespace VolunteerMatch.Controllers;

[ApiController]
[Route("api/")]
public class OrganizationFollowerController : ControllerBase
{
  private readonly IOrganizationFollowerRepository _organizationFollowerRepository;

  public OrganizationFollowerController(IOrganizationFollowerRepository organizationFollowerRepository)
  {
    _organizationFollowerRepository = organizationFollowerRepository;
  }

  [HttpGet]
  [Route("organizations/{organizationId:int}/followers")]
  public async Task<ActionResult<List<OrganizationFollower>>> GetFollowersByOrganizationIdAsync(int organizationId)
  {
    var followers = await _organizationFollowerRepository.GetFollowersByOrganizationIdAsync(organizationId);
    return Ok(followers);
  }
  
  [HttpGet]
  [Route("volunteers/{volunteerId:int}/organizations-following")]
  public async Task<ActionResult<List<Organization>>> GetOrganizationsUserIsFollowingTask(int volunteerId)
  {
    var following = await _organizationFollowerRepository.GetOrganizationsUserIsFollowingTask(volunteerId);
    return Ok(following);
  }
}