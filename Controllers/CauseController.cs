using Microsoft.AspNetCore.Mvc;
using VolunteerMatch.Data.Repositories.Interfaces;
using VolunteerMatch.Models;

namespace VolunteerMatch.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CauseController : ControllerBase
{
  private readonly ICauseRepository _causeRepository;

  public CauseController(ICauseRepository causeRepository)
  {
    _causeRepository = causeRepository;
  }

  [HttpGet]
  public async Task<IActionResult> GetAllAsyncTask()
  {
    var causes = await _causeRepository.GetAllAsync();
    return Ok(causes);
  }

  [HttpGet("{id:int}")]
  public async Task<IActionResult> GetByIdTask(int id)
  {
    var cause = await _causeRepository.GetByIdAsync(id);
    return Ok(cause);
  }

  [HttpPost]
  public async Task<IActionResult> PostAsyncTask(Cause cause)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }
    
    await _causeRepository.AddAsync(cause);
    return CreatedAtAction("GetByIdTask", new { id = cause.Id }, cause);
  }

  [HttpPatch("{id:int}")]
  public async Task<IActionResult> PatchAsyncTask(int id, [FromBody] Cause cause)
  {
    if (id != cause.Id)
    {
      return BadRequest();
    }
    
    var causeToUpdate = await _causeRepository.GetByIdAsync(cause.Id);

    if (causeToUpdate == null)
    {
      return NotFound();
    }
    
    causeToUpdate.Name = cause.Name;
    causeToUpdate.ImageUrl = cause.ImageUrl;
    causeToUpdate.Description = cause.Description;
    
    await _causeRepository.UpdateAsync(causeToUpdate);
    return Ok(causeToUpdate);
  }

  [HttpDelete("{id:int}")]
  public async Task<IActionResult> DeleteAsyncTask(int id)
  {
    var cause = await _causeRepository.GetByIdAsync(id);

    if (cause != null) await _causeRepository.DeleteAsync(cause.Id);
    return NoContent();
  }
}