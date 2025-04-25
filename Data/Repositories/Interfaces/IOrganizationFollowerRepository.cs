using VolunteerMatch.Models;

namespace VolunteerMatch.Data.Repositories.Interfaces;

public interface IOrganizationFollowerRepository
{
  Task<List<Organization>> GetFollowersByOrganizationIdAsync(int organizationId);
  // We can add more as needed.
}