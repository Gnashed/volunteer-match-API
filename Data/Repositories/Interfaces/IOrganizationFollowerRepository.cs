using VolunteerMatch.Models;

namespace VolunteerMatch.Data.Repositories.Interfaces;

public interface IOrganizationFollowerRepository
{
  Task<ICollection<OrganizationFollower>> GetAllAsync();
  Task<ICollection<OrganizationFollower>> GetFollowersByOrganizationIdAsync(int organizationId);
  Task<OrganizationFollower> GetByIdAsync(int volunteerId, int organizationId);
  Task DeleteAsync(OrganizationFollower organizationFollower);
  // We can add more as needed.
}