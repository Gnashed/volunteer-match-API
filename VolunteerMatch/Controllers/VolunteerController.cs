using Microsoft.AspNetCore.Mvc;
using VolunteerMatch.Data.Repositories.Interfaces;
using VolunteerMatch.Models;
using VolunteerMatch.Models.DTOs;
using VolunteerMatch.Models.Requests;

namespace VolunteerMatch.Controllers;

[ApiController]
[Route("api/volunteer")]
public class VolunteerController : ControllerBase
{
  private readonly IVolunteerRepository _volunteerRepository;

  public VolunteerController(IVolunteerRepository volunteerRepository)
  {
    _volunteerRepository = volunteerRepository;
  }

  [HttpGet]
  public async Task<IActionResult> GetAllAsyncTask()
  {
    var volunteers = await _volunteerRepository.GetAllAsync();
    var volunteersDtos = volunteers.Select(v => new VolunteerDto
    {
      Id = v.Id,
      Uid = v.Uid,
      FirstName = v.FirstName,
      LastName = v.LastName,
      Email = v.Email,
      ImageUrl = v.ImageUrl
    }).ToList();
    
    return Ok(volunteersDtos);
  }

  [HttpGet("{id:int}")]
  public async Task<IActionResult> GetByIdAsyncTask(int id)
  {
    var volunteer = await _volunteerRepository.GetByIdAsync(id);

    if (volunteer == null)
    {
      return NotFound();
    }
    
    var volunteerDto = new VolunteerDto
    {
      Id = volunteer.Id,
      Uid = volunteer.Uid,
      FirstName = volunteer.FirstName,
      LastName = volunteer.LastName,
      Email = volunteer.Email,
      ImageUrl = volunteer.ImageUrl
    };
    
    return Ok(volunteerDto);
  }

  [HttpPost]
  public async Task<IActionResult> PostAsyncTask([FromBody] CreateVolunteerRequest request)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    var volunteer = new Volunteer
    {
      FirstName = request.FirstName,
      LastName = request.LastName,
      Email = request.Email,
      ImageUrl = request.ImageUrl,
      Uid = request.Uid
    };
    
    await _volunteerRepository.AddAsync(volunteer);

    var volunteerDto = new VolunteerDto
    {
      Id = volunteer.Id,
      Uid = volunteer.Uid,
      FirstName = request.FirstName,
      LastName = request.LastName,
      Email = request.Email,
      ImageUrl = request.ImageUrl,
    };
    
    return CreatedAtAction(nameof(GetByIdAsyncTask), new { id = volunteer.Id }, volunteerDto);
  }

  [HttpPatch("{id:int}")]
  public async Task<IActionResult> PatchAsyncTask(int id, [FromBody] UpdateVolunteerRequest request)
  {
    var volunteerToUpdate = await _volunteerRepository.GetByIdAsync(id);
    
    volunteerToUpdate.FirstName = request.FirstName;
    volunteerToUpdate.LastName = request.LastName;
    volunteerToUpdate.Email = request.Email;
    volunteerToUpdate.ImageUrl = request.ImageUrl;
    
    await _volunteerRepository.UpdateAsync(volunteerToUpdate);

    var volunteerDto = new VolunteerDto
    {
      Id = volunteerToUpdate.Id,
      Uid = volunteerToUpdate.Uid,
      FirstName = volunteerToUpdate.FirstName,
      LastName = volunteerToUpdate.LastName,
      Email = volunteerToUpdate.Email,
      ImageUrl = volunteerToUpdate.ImageUrl
    };
    return Ok(volunteerDto);
  }

  [HttpDelete]
  public async Task<IActionResult> DeleteAsyncTask(int id)
  {
    var volunteer = await _volunteerRepository.GetByIdAsync(id);

    await _volunteerRepository.DeleteAsync(volunteer.Id);
    return NoContent();
  }
}