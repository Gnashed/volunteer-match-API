using VolunteerMatch.Models;

namespace VolunteerMatch.Data.Repositories.Interfaces;

public interface IOrganizationRepository
{
  Task<ICollection<Organization>> GetAllAsync();
  Task<Organization?> GetByIdAsync(int id);
  Task AddAsync(Organization organization);
  Task UpdateAsync(Organization organization);
  Task DeleteAsync(int id);
}