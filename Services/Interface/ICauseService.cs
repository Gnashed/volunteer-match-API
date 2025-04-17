using VolunteerMatch.Models;

namespace VolunteerMatch.Services.Interface;

public interface ICauseService
{
  Task<Cause?> GetByIdAsync(int id);
  Task<Cause> AddAsync(Cause cause);
  Task<Cause> UpdateAsync(Cause cause);
  Task<Cause> DeleteAsync(int id);
  Task<IEnumerable<Cause>> GetAllAsync();
}