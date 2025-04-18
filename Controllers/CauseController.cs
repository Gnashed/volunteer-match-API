using Microsoft.AspNetCore.Mvc;
using VolunteerMatch.Data.Repositories.Interfaces;

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
}