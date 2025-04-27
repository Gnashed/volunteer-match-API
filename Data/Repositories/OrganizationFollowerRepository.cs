using Microsoft.EntityFrameworkCore;
using VolunteerMatch.Data.Repositories.Interfaces;
using VolunteerMatch.Models;

namespace VolunteerMatch.Data.Repositories;

public class OrganizationFollowerRepository : IOrganizationFollowerRepository
{
  private readonly VolunteerMatchDbContext _context;

  public OrganizationFollowerRepository(VolunteerMatchDbContext context)
  {
    _context = context;
  }

  public async Task<List<OrganizationFollower>> GetFollowersByOrganizationIdAsync(int organizationId)
  {
    return await _context.OrganizationFollowers
      .Where(of => of.OrganizationId == organizationId)
      .ToListAsync();
  }

  public async Task<List<Organization>> GetOrganizationsUserIsFollowingTask(int volunteerId)
  {
     return await _context.Organizations
       .Where(o => o.OrganizationFollowers.Any(of => of.VolunteerId == volunteerId))
       .ToListAsync();
  }
}