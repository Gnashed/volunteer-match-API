using Microsoft.AspNetCore.Mvc;
using VolunteerMatch.Data.Repositories.Interfaces;
using VolunteerMatch.Models;

namespace VolunteerMatch.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    return Ok(await _volunteerRepository.GetAllAsync());
  }

  [HttpGet("{id:int}")]
  public async Task<IActionResult> GetByIdAsyncTask(int id)
  {
    var volunteer = await _volunteerRepository.GetByIdAsync(id);
    return Ok(volunteer);
  }

  [HttpPost]
  public async Task<IActionResult> PostAsyncTask(Volunteer volunteer)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }
    
    await _volunteerRepository.AddAsync(volunteer);
    return CreatedAtAction(nameof(GetByIdAsyncTask), new { id = volunteer.Id }, volunteer);
  }

  [HttpPatch("{id:int}")]
  public async Task<IActionResult> PatchAsyncTask(int id, [FromBody] Volunteer volunteer)
  {
    if (id != volunteer.Id)
    {
      return BadRequest();
    }
    
    var volunteerToUpdate = await _volunteerRepository.GetByIdAsync(volunteer.Id);
    
    volunteerToUpdate.FirstName = volunteer.FirstName;
    volunteerToUpdate.LastName = volunteer.LastName;
    volunteerToUpdate.Email = volunteer.Email;
    volunteerToUpdate.ImageUrl = volunteer.ImageUrl;
    
    await _volunteerRepository.UpdateAsync(volunteerToUpdate);
    return Ok(volunteer);
  }

  [HttpDelete]
  public async Task<IActionResult> DeleteAsyncTask(int id)
  {
    var volunteer = await _volunteerRepository.GetByIdAsync(id);

    await _volunteerRepository.DeleteAsync(volunteer.Id);
    return NoContent();
  }
}