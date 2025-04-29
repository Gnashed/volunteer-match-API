using VolunteerMatch.Models;

namespace VolunteerMatch.Data.Repositories.Interfaces;

public interface IOrganizationFollowerRepository
{
  Task<List<OrganizationFollower>> GetFollowersByOrganizationIdAsync(int organizationId);
  Task<List<Organization>> GetOrganizationsUserIsFollowingTask(int volunteerId);
  // We can add more as needed.
}