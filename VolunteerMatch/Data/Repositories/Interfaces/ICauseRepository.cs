using VolunteerMatch.Models;

namespace VolunteerMatch.Data.Repositories.Interfaces;

public interface ICauseRepository
{
  Task<ICollection<Cause>> GetAllAsync();
  Task<Cause?> GetByIdAsync(int id);
  Task AddAsync(Cause cause);
  Task UpdateAsync(Cause cause);
  Task DeleteAsync(int id);
}