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
  public async Task<IActionResult> GetAllAsync()
  {
    var organizations = await _organizationRepository.GetAllAsync();
    return Ok(organizations);
  }
}