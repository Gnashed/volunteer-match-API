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
        VolunteerId = 1
      },
      new OrganizationFollower
      {
        OrganizationId = 2,
        VolunteerId = 1
      },
      new OrganizationFollower
      {
        OrganizationId = 3,
        VolunteerId = 1
      },
      new OrganizationFollower
      {
        OrganizationId = 4,
        VolunteerId = 1
      },
      new OrganizationFollower
      {
        OrganizationId = 1,
        VolunteerId = 5
      },
      new OrganizationFollower
      {
        OrganizationId = 5,
        VolunteerId = 6
      },
      new OrganizationFollower
      {
        OrganizationId = 5,
        VolunteerId = 7
      },
      new OrganizationFollower
      {
        OrganizationId = 5,
        VolunteerId = 8
      },
      new OrganizationFollower
      {
        OrganizationId = 5,
        VolunteerId = 9
      },
      new OrganizationFollower
      {
        OrganizationId = 5,
        VolunteerId = 10
      }
    };
  }
}