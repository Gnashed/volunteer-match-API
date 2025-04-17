using Microsoft.AspNetCore.Mvc;
using VolunteerMatch.Data.Repositories.Interfaces;

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
  public async Task<IActionResult> GetAll()
  {
    var products = await _causeRepository.GetAllAsync();
    return Ok(products);
  }
}