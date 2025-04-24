using Microsoft.EntityFrameworkCore;
using VolunteerMatch.Data.Repositories.Interfaces;
using VolunteerMatch.Models;

namespace VolunteerMatch.Data.Repositories;

public class CauseRepository : ICauseRepository
{
  // Remember that 'readonly' means this variable can be assigned only once when declared or inside the constructor of
  // this class. Also remember the object that it references can change.
  private readonly VolunteerMatchDbContext _context;
  
  // context will be the DBContext instance.
  public CauseRepository(VolunteerMatchDbContext context)
  {
    // We're assigning the incoming context param to _context.
    _context = context;
  }
  
  public async Task<ICollection<Cause>> GetAllAsync() => await _context.Causes.ToListAsync();

  public async Task<Cause?> GetByIdAsync(int id)
  {
    var cause = _context.Causes.FindAsync(id);
    return await cause;
  }

  public async Task AddAsync(Cause cause)
  {
    await _context.Causes.AddAsync(cause);
    await _context.SaveChangesAsync();
  }

  public async Task UpdateAsync(Cause cause)
  {
    var existingCause = await _context.Causes.FindAsync(cause.Id);
    existingCause.Name =  cause.Name;
    existingCause.Description = cause.Description;
    existingCause.ImageUrl = cause.ImageUrl;
    await _context.SaveChangesAsync();
  }

  public async Task DeleteAsync(int id)
  {
    var causeToDelete = await _context.Causes.FindAsync(id);
    if (causeToDelete == null)
    {
      throw new Exception($"Cause with id {id} was not found");
    }
    _context.Causes.Remove(causeToDelete);
    await _context.SaveChangesAsync();
  }
}