using Microsoft.AspNetCore.Mvc;
using VolunteerMatch.Data.Repositories.Interfaces;
using VolunteerMatch.Models;
using VolunteerMatch.Models.DTOs;
using VolunteerMatch.Models.Requests;

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
  public async Task<IActionResult> AddAsyncTask([FromBody] CreateOrganizationRequest request)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }
    
    var organization = new Organization
    {
      Name = request.Name,
      Description = request.Description,
      ImageURL = request.ImageURL,
      Location = request.Location,
      IsFollowing = request.IsFollowing,
      CauseId = request.CauseId
    };
    
    await _organizationRepository.AddAsync(organization);
    
    var organizationDto = new OrganizationDto
    {
      Id = organization.Id,
      Name = organization.Name,
      Description = organization.Description,
      ImageURL = organization.ImageURL,
      Location = organization.Location,
      IsFollowing = organization.IsFollowing,
      CauseId = organization.CauseId
    };
    
    return CreatedAtAction(nameof(GetByIdTask), new  { id = organization.Id }, organizationDto);
  }

  [HttpPatch("{id:int}")]
  public async Task<IActionResult> UpdateAsTask(int id, [FromBody] UpdateOrganizationRequest request)
  {
    var organizationToUpdate = await _organizationRepository.GetByIdAsync(id);
    
    if (organizationToUpdate == null)
    {
      return NotFound();
    }

    organizationToUpdate.Name = request.Name;
    organizationToUpdate.ImageURL = request.ImageURL;
    organizationToUpdate.Description = request.Description;
    organizationToUpdate.Location = request.Location;
    organizationToUpdate.IsFollowing = request.IsFollowing;
    
    var organizationDto = new OrganizationDto
    {
      Id = organizationToUpdate.Id,
      Name = organizationToUpdate.Name,
      Description = organizationToUpdate.Description,
      ImageURL = organizationToUpdate.ImageURL,
      Location = organizationToUpdate.Location,
      IsFollowing = organizationToUpdate.IsFollowing,
      CauseId = organizationToUpdate.CauseId
    };
    
    await _organizationRepository.UpdateAsync(organizationToUpdate);
    return Ok(organizationDto);
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