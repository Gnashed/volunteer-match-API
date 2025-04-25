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
  
  public async Task<List<Organization>> GetAllAsync()
  {
    return await _context.Organizations
      .Include(o => o.OrganizationFollowers)
      .ToListAsync();
  }

  public async Task<List<Organization>> GetFollowersByOrganizationIdAsync(int organizationId)
  {
    return await _context.Organizations
      .Where(o => o.Id == organizationId)
      .Include(o => o.OrganizationFollowers)
      .ToListAsync();
  }
}