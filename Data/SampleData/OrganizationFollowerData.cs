using VolunteerMatch.Models;

namespace VolunteerMatch.Data.SampleData;

public static class OrganizationFollowerData
{
  public static List<OrganizationFollower> GetOrganizationFollowers()
  {
    return new List<OrganizationFollower>
    {
      new OrganizationFollower
      {
        OrganizationId = 1,
        VolunteerId = 0
      }
    };
  }
}