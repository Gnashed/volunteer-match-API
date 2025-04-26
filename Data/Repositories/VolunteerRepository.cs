using Microsoft.EntityFrameworkCore;
using VolunteerMatch.Data.Repositories.Interfaces;
using VolunteerMatch.Models;

namespace VolunteerMatch.Data.Repositories;

public class VolunteerRepository : IVolunteerRepository
{
  private readonly VolunteerMatchDbContext _context;

  public VolunteerRepository(VolunteerMatchDbContext context)
  {
    _context = context;
  }

  // Controller action methods
  public async Task<ICollection<Volunteer>> GetAllAsync() => await _context.Volunteers.ToListAsync();

  public async Task<Volunteer> GetByIdAsync(int id)
  {
    var volunteer = await _context.Volunteers.FindAsync(id);
    if (volunteer == null)
    {
      throw new Exception($"Error in repository - Volunteer with id {id} was not found");
    }
    return volunteer;
  }

  public async Task AddAsync(Volunteer volunteer)
  {
    await _context.Volunteers.AddAsync(volunteer);
    await _context.SaveChangesAsync();
  }

  public async Task UpdateAsync(Volunteer volunteer)
  {
    var existingVolunteer = await _context.Volunteers.FindAsync(volunteer.Id);
    existingVolunteer.FirstName = volunteer.FirstName;
    existingVolunteer.LastName = volunteer.LastName;
    existingVolunteer.Email = volunteer.Email;
    existingVolunteer.ImageUrl = volunteer.ImageUrl;
    await _context.SaveChangesAsync();
  }

  public async Task DeleteAsync(int id)
  {
    var volunteerToDelete = await _context.Volunteers.FindAsync(id);
    if (volunteerToDelete == null)
    {
      throw new Exception($"Error in repository - Volunteer with id {id} not found");
    }
    _context.Volunteers.Remove(volunteerToDelete);
    await _context.SaveChangesAsync();
  }
}
