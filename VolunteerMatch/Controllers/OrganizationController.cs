using Microsoft.AspNetCore.Mvc;
using VolunteerMatch.Data.Repositories.Interfaces;
using VolunteerMatch.Models;
using VolunteerMatch.Models.DTOs;
using VolunteerMatch.Models.Requests;

namespace VolunteerMatch.Controllers;

[ApiController]
[Route("api/organization")]
public class OrganizationController : ControllerBase
{
  private readonly IOrganizationRepository _organizationRepository;

  public OrganizationController(IOrganizationRepository organizationRepository)
  {
    _organizationRepository = organizationRepository;
  }

  [HttpGet]
  [Route("/api/organizations")]
  public async Task<IActionResult> GetAllAsync()
  {
    var organizations = await _organizationRepository.GetAllAsync();
    var organizationsDtos = organizations.Select(o => new OrganizationDto
    {
      Id = o.Id,
      CauseId = o.CauseId,
      Description = o.Description,
      ImageURL = o.ImageURL,
      IsFollowing = o.IsFollowing,
      Name = o.Name,
      Location = o.Location
    });
    
    return Ok(organizationsDtos);
  }
  
  [HttpGet("{id:int}")]
  public async Task<IActionResult> GetByIdTask(int? id)
  {
    var organization = await _organizationRepository.GetByIdAsync(id.Value);

    if (organization == null)
    {
      return NotFound();
    }

    var organizationDto = new OrganizationDto
    {
      Id = organization.Id,
      Name = organization.Name,
      ImageURL = organization.ImageURL,
      Description = organization.Description,
      Location = organization.Location,
      IsFollowing = organization.IsFollowing,
      CauseId = organization.CauseId
    };
    return Ok(organizationDto);
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