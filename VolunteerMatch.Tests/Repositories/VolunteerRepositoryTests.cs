using Microsoft.EntityFrameworkCore;
using VolunteerMatch.Data;
using VolunteerMatch.Data.Repositories;
using VolunteerMatch.Models;

namespace VolunteerMatch.Tests.Repositories;
public class VolunteerRepositoryTests
{
  // Helper method
  private VolunteerMatchDbContext GetDbContext()
  {
    var options = new DbContextOptionsBuilder<VolunteerMatchDbContext>()
      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
      .Options;

    return new VolunteerMatchDbContext(options);
  }

  [Fact]
  public async Task GetAllAsync_ReturnsAllVolunteers()
  {
    // Arrange
    var db = GetDbContext();
    db.Volunteers.AddRange(new List<Volunteer>
    {
      new Volunteer { Id = 1, Uid = "fjf3h87g322bg8ypoDgh", FirstName = "Susan", LastName = "Beth", Email = "susan_beth@email.com", ImageUrl = "profile1.jpg"},
      new Volunteer { Id = 2, Uid = "plJ3h87g311bg8yMMnw7", FirstName = "Eric", LastName = "Denim", Email = "eric_denim@email.com", ImageUrl = "profile2.jpg"},
      new Volunteer { Id = 3, Uid = "XXj3h87g999bg8y09db2", FirstName = "Janiyah", LastName = "Basin", Email = "janiyah_basin@email.com", ImageUrl = "profile3.jpg"}, 
    });
    await db.SaveChangesAsync();
    var repository = new VolunteerRepository(db);
    
    // Act
    var result = await repository.GetAllAsync();
    
    // Assert
    Assert.Equal(3, result.Count);
  }

  [Fact]
  public async Task GetByIdAsync_ReturnsVolunteer()
  {
    // Arrange
    var db = GetDbContext();
    var volunteer = new Volunteer 
    {
      Id = 1,
      Uid = "fjf3h87g322bg8ypoDgh",
      FirstName = "Susan",
      LastName = "Beth",
      Email = "susan_beth@email.com",
      ImageUrl = "profile1.jpg"
    };
    await db.Volunteers.AddAsync(volunteer);
    await db.SaveChangesAsync();
    
    var repository = new VolunteerRepository(db);
    
    // Act
    var result = await repository.GetByIdAsync(1);
    
    // Assert
    Assert.NotNull(result);
    Assert.Equal("Susan", result.FirstName);
  }

  [Fact]
  public async Task GetByIdAsync_ReturnsNullWhenNotFound()
  {
    // Arrange
    var db = GetDbContext();
    var repository = new VolunteerRepository(db);
    
    // Act
    var volunteer = await repository.GetByIdAsync(999);
    
    // Assert
    Assert.Null(volunteer);
  }

  [Fact]
  public async Task AddAsync_AddsNewVolunteer()
  {
    // Arrange
    var db = GetDbContext();
    var repository = new VolunteerRepository(db);
    var newVolunteer = new Volunteer
    {
      Id = 2,
      Uid = "fjf3h87g322bg8ypoDgh",
      FirstName = "Eric",
      LastName = "Denim",
      Email = "eric_denim@email.com",
      ImageUrl = "profile2.jpg"
    };
    
    // Act
    await repository.AddAsync(newVolunteer);
    await db.SaveChangesAsync();
    
    // Assert
    await repository.GetByIdAsync(2);
    Assert.NotNull(newVolunteer);
    Assert.Equal("Eric", newVolunteer.FirstName);
  }

  [Fact]
  public async Task UpdateAsync_UpdatesVolunteer()
  {
    // Arrange
    var db = GetDbContext();
    var originalVolunteer = new Volunteer
    {
      Id = 2,
      Uid = "fjf3h87g322bg8ypoDgh",
      FirstName = "Eir",
      LastName = "Denim",
      Email = "eric_denim@email.com",
      ImageUrl = "profile2.jpg"
    };
    db.Volunteers.Add(originalVolunteer);
    await db.SaveChangesAsync();
    
    var repository = new VolunteerRepository(db);
    originalVolunteer.FirstName = "Eric";
    
    // Act
    await repository.UpdateAsync(originalVolunteer);
    await db.SaveChangesAsync();
    
    // Assert
    var updatedVolunteer = await repository.GetByIdAsync(2);
    Assert.NotNull(updatedVolunteer);
    Assert.Equal("Eric", updatedVolunteer.FirstName);
  }

  [Fact]
  public async Task DeleteAsync_DeletesVolunteer()
  {
    // Arrange
    var db = GetDbContext();
    var volunteer = new Volunteer
    {
      Id = 3,
      Uid = "XXj3h87g999bg8y09db2",
      FirstName = "Janiyah",
      LastName = "Basin",
      Email = "janiyah_basin@email.com",
      ImageUrl = "profile3.jpg"
    };
    await db.Volunteers.AddAsync(volunteer);
    await db.SaveChangesAsync();
    
    var repository = new VolunteerRepository(db);

    // Act
    await repository.DeleteAsync(3);

    // Assert
    var deletedVolunteer = await db.Volunteers.FindAsync(3);
    Assert.Null(deletedVolunteer);
  }
}