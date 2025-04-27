using Microsoft.AspNetCore.Mvc;
using VolunteerMatch.Data.Repositories.Interfaces;
using VolunteerMatch.Models;
using VolunteerMatch.Models.DTOs;
using VolunteerMatch.Models.Requests;

namespace VolunteerMatch.Controllers;

[ApiController]
[Route("api/cause")]
public class CauseController : ControllerBase
{
  private readonly ICauseRepository _causeRepository;

  public CauseController(ICauseRepository causeRepository)
  {
    _causeRepository = causeRepository;
  }

  [HttpGet]
  [Route("/api/causes")]
  public async Task<IActionResult> GetAllAsyncTask()
  {
    var causes = await _causeRepository.GetAllAsync();
    
    // Mapping list to dto.
    var causeDtos = causes.Select(c => new CauseDto
    {
      Id = c.Id,
      Name = c.Name,
      ImageUrl = c.ImageUrl,
      Description = c.Description
    }).ToList();
    
    return Ok(causeDtos);
  }

  [HttpGet("{id:int}")]
  public async Task<IActionResult> GetByIdTask(int id)
  {
    var cause = await _causeRepository.GetByIdAsync(id);
    
    // Check before mapping...
    if (cause == null)
    {
      return NotFound();
    }
    
    // Mapping DTO to what we want to expose to the client...
    var causeDto = new CauseDto
    {
      Id = cause.Id,
      Name = cause.Name,
      Description = cause.Description,
      ImageUrl = cause.ImageUrl
    };
    
    return Ok(causeDto);
  }

  [HttpPost]
  public async Task<IActionResult> PostAsyncTask([FromBody] CreateCauseRequest request)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    var cause = new Cause
    {
      Name = request.Name,
      Description = request.Description,
      ImageUrl = request.ImageUrl
    };
    
    await _causeRepository.AddAsync(cause);

    var causeDto = new CauseDto
    {
      Id = cause.Id,
      Name = cause.Name,
      Description = cause.Description,
      ImageUrl = cause.ImageUrl
    };
    
    return CreatedAtAction("GetByIdTask", new { id = cause.Id }, causeDto);
  }

  [HttpPatch("{id:int}")]
  public async Task<IActionResult> PatchAsyncTask(int id, [FromBody] UpdateCauseRequest request)
  {
    var causeToUpdate = await _causeRepository.GetByIdAsync(id);

    if (causeToUpdate == null)
    {
      return NotFound();
    }
    
    causeToUpdate.Name = request.Name;
    causeToUpdate.ImageUrl = request.ImageUrl;
    causeToUpdate.Description = request.Description;
    
    await _causeRepository.UpdateAsync(causeToUpdate);

    var causeDto = new CauseDto
    {
      Id = causeToUpdate.Id,
      Name = causeToUpdate.Name,
      Description = causeToUpdate.Description,
      ImageUrl = causeToUpdate.ImageUrl
    };
    
    return Ok(causeDto);
  }

  [HttpDelete("{id:int}")]
  public async Task<IActionResult> DeleteAsyncTask(int id)
  {
    var cause = await _causeRepository.GetByIdAsync(id);

    if (cause != null) await _causeRepository.DeleteAsync(cause.Id);
    return NoContent();
  }
}