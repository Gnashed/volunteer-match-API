using Microsoft.EntityFrameworkCore;
using VolunteerMatch.Data.Repositories.Interfaces;
using VolunteerMatch.Models;

namespace VolunteerMatch.Data.Repositories;

public class OrganizationRepository : IOrganizationRepository
{
  private readonly VolunteerMatchDbContext _context;
  public OrganizationRepository(VolunteerMatchDbContext context)
  {
    _context = context;
  }
  
  public async Task<ICollection<OrganizationFollower>> GetAllAsync() => await _context.OrganizationFollowers.ToListAsync();

  public async Task<Organization?> GetByIdAsync(int id)
  {
    var organization = await _context.Organizations.FindAsync(id);
    return organization;
  }

  public async Task AddAsync(Organization organization)
  {
    await _context.Organizations.AddAsync(organization);
    await _context.SaveChangesAsync();
  }

  public async Task UpdateAsync(Organization organization)
  {
    var existingOrganization = await _context.Organizations.FindAsync(organization.Id);
    existingOrganization.Name = organization.Name;
    existingOrganization.Description = organization.Description;
    existingOrganization.ImageURL = organization.ImageURL;
    existingOrganization.IsFollowing = organization.IsFollowing;
    existingOrganization.Location = organization.Location;
    await _context.SaveChangesAsync();
  }

  public async Task DeleteAsync(int id)
  {
    var organizationToDelete = await _context.Organizations.FindAsync(id);
    if (organizationToDelete == null)
    {
      throw new Exception($"Error in repository - Organization with id {id} not found");
    }
    _context.Organizations.Remove(organizationToDelete);
    await _context.SaveChangesAsync();
  }
}