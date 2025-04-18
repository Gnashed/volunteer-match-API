using Microsoft.AspNetCore.Mvc;
using VolunteerMatch.Data.Repositories.Interfaces;

namespace VolunteerMatch.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrganizationController : ControllerBase
{
  private readonly IOrganizationRepository _organizationRepository;

  public OrganizationController(IOrganizationRepository organizationRepository)
  {
    _organizationRepository = organizationRepository;
  }

  [HttpGet]
  public async Task<IActionResult> GetAllAsyncTask()
  {
    var organizationFollowers = await _organizationRepository.GetAllAsync();
    return Ok(organizationFollowers);
  }

  [HttpGet("{id:int}")]
  public async Task<IActionResult> GetByIdTask(int? id)
  {
    var organization = await _organizationRepository.GetByIdAsync(id.Value);
    return Ok(organization);
  }
}