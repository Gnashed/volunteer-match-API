using VolunteerMatch.Models;

namespace VolunteerMatch.Data.Repositories.Interfaces;

public interface IVolunteerRepository
{
  Task<ICollection<Organization>> GetAllAsync();
  Task<Volunteer> GetByIdAsync(int id);
  Task AddAsync(Volunteer volunteer);
  Task UpdateAsync(Volunteer volunteer);
  Task DeleteAsync(int id);
}