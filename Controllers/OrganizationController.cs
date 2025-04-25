using Microsoft.AspNetCore.Mvc;
using VolunteerMatch.Data.Repositories.Interfaces;
using VolunteerMatch.Models;

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

  // [HttpGet]
  // public async Task<IActionResult> GetAllAsyncTask()
  // {
  //   var organizationFollowers = await _organizationRepository.GetAllAsync();
  //   return Ok(organizationFollowers);
  // }

  [HttpGet("{id:int}")]
  public async Task<IActionResult> GetByIdTask(int? id)
  {
    var organization = await _organizationRepository.GetByIdAsync(id.Value);
    return Ok(organization);
  }

  [HttpPost]
  public async Task<IActionResult> AddAsyncTask([FromBody]Organization organization)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }
    
    await _organizationRepository.AddAsync(organization);
    return CreatedAtAction(nameof(GetByIdTask), new  { id = organization.Id }, organization);
  }

  [HttpPatch("{id:int}")]
  public async Task<IActionResult> UpdateAsTask(int id, [FromBody] Organization organization)
  {
    if (id != organization.Id)
    {
      return BadRequest();
    }
    
    var organizationToUpdate = await _organizationRepository.GetByIdAsync(organization.Id);
    if (organizationToUpdate == null)
    {
      return NotFound();
    }
    
    organizationToUpdate.Name = organization.Name;
    organizationToUpdate.ImageURL = organization.ImageURL;
    organizationToUpdate.Description = organization.Description;
    organizationToUpdate.Location = organization.Location;
    organizationToUpdate.IsFollowing = organization.IsFollowing;
    
    await _organizationRepository.UpdateAsync(organizationToUpdate);
    return Ok(organizationToUpdate);
  }

  [HttpDelete("{id:int}")]
  public async Task<IActionResult> DeleteASyncTask(int id)
  {
    var organizationToDelete = await _organizationRepository.GetByIdAsync(id);
    if (organizationToDelete == null)
    {
      return NotFound();
    }

    await _organizationRepository.DeleteAsync(organizationToDelete.Id);
    return NoContent();
  }
}