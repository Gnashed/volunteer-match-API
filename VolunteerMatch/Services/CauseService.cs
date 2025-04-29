using VolunteerMatch.Models;
using VolunteerMatch.Data.Repositories.Interfaces;
using VolunteerMatch.Services.Interface;

namespace VolunteerMatch.Services;

public class CauseService : ICauseService
{
  private readonly ICauseRepository _causeRepository;

  public CauseService(ICauseRepository causeRepository)
  {
    _causeRepository = causeRepository;
  }

  public async Task<Cause?> GetByIdAsync(int id)
  {
    return await _causeRepository.GetByIdAsync(id);
  }

  public async Task<Cause> AddAsync(Cause cause)
  {
    await _causeRepository.AddAsync(cause);
    return cause;
  }

  public async Task<Cause> UpdateAsync(Cause cause)
  {
    await _causeRepository.UpdateAsync(cause);
    return cause;
  }

  public async Task<Cause> DeleteAsync(int id)
  {
    var causeToDelete = await _causeRepository.GetByIdAsync(id);
    if (causeToDelete == null)
    {
      throw new Exception($"Unable to delete the cause. Cause with id {id} was not found");
    }

    await _causeRepository.DeleteAsync(id);
    return causeToDelete;
  }

  public async Task<IEnumerable<Cause>> GetAllAsync()
  {
    return await _causeRepository.GetAllAsync();
  }
}