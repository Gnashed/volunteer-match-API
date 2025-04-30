// When testing a concrete repository, to avoid testing the actual database you can use the UseInMemoryDatabase to create a InMemory DbContext.
using Xunit;
using Moq;
using VolunteerMatch.Data.Repositories;
using VolunteerMatch.Data;
using VolunteerMatch.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace VolunteerMatch.Tests.Repositories;

public class CauseRepositoryTests
{
  /*
   This is a helper method that builds and returns a new in-memory database context using VolunteerMatchDbContext
   from the data-access layer. We do this to avoid hitting our actual database during tests. Every test will use a fresh 
   instance thanks to using Guid to create a unique database name.   
  */
  private VolunteerMatchDbContext GetDbContext()
  {
    var options = new DbContextOptionsBuilder<VolunteerMatchDbContext>()
      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
      .Options;

    return new VolunteerMatchDbContext(options);
  }
  
  /*
   * This is an xUnit test case. This method follows the common pattern for naming your
   * test methods --- MethodName_ExpectedBehavior
   *
   * Also, the `[Fact]` attribute tells xUnit to treat it as a test that takes no parameters.
   *
   * The test method will verify that CausesRepository.GetAllAsync() correctly returns all causes from the database.
   * When combining it with in-memory database context, you are creating tests that stays fresh and isolated from real
   * databases/external services.
   * 
   */
  [Fact]
  public async Task GetAllAsync_ReturnsAllCauses()
  {
    // Arrange - Set up the test scenario
    var db = GetDbContext();
    db.Causes.AddRange(new List<Cause>
    {
      new Cause {Id = 1, Name = "Test Cause 1", Description = "The first test instance.", ImageUrl = "image.jpg"},
      new Cause {Id = 2, Name = "Test Cause 2", Description = "The second test instance.",  ImageUrl = "image2.jpg"},
      new Cause{ Id = 3, Name = "Test Cause 3", Description = "The third test instance.",  ImageUrl = "image3.jpg" }
    });
    await db.SaveChangesAsync();
    var repository = new CauseRepository(db);
    
    // Act - Call your method/function you are testing.
    var causes = await repository.GetAllAsync();

    // Assert - Check whether the number of causes returned is 3, exactly as seeded in the Arrange block above.
    Assert.Equal(3, causes.Count);
  }

  [Fact]
  public async Task GetCauseById_ReturnsCause()
  {
    // Arrange
    var db = GetDbContext();
    var cause = new Cause
    {
      Id = 1,
      Name = "Test for Single Cause", 
      Description = "Testing retrieving a Cause", 
      ImageUrl = "causeImage.jpg"
    };
    db.Causes.Add(cause);
    await db.SaveChangesAsync();

    var repository = new CauseRepository(db);
    
    // Act
    var result = await repository.GetByIdAsync(1);
    
    // Assert
    Assert.NotNull(result);
    Assert.Equal("Test for Single Cause", result.Name);
  }

  [Fact]
  public async Task GetCauseByIdAsync_ReturnNullWhenCaseNotFound()
  {
    // Arrange
    var db = GetDbContext();
    var repository = new CauseRepository(db);
  
    // Act
    var result = await repository.GetByIdAsync(1092);
    
    // Assert
    Assert.Null(result);
  }
  
  [Fact]
  public async Task AddCauseAsync_AddsNewCause()
  {
    // Arrange
    var db = GetDbContext();
    var repository = new CauseRepository(db);
    var newCause = new Cause
    {
      Id = 1,
      Name = "Add Cause (Test)",
      Description = "Testing adding a Cause",
      ImageUrl = "causeImage.jpg"
    };

    // Act
    await repository.AddAsync(newCause);
    
    // Assert
    var addedCause = await repository.GetByIdAsync(1);
    Assert.NotNull(addedCause);
    Assert.Equal("Add Cause (Test)", addedCause.Name);
  }

  [Fact]
  public async Task UpdateCauseAsync_UpdatesExistingCause()
  {
    // Arrange
    var db = GetDbContext();
    var originalCause = new Cause { Id = 1, Name = "Physical Health", Description = "This is a test for updating a cause", ImageUrl = "causeImage.jpg" };
    db.Causes.Add(originalCause);
    await db.SaveChangesAsync();

    var repository = new CauseRepository(db);
    originalCause.Name = "Mental Health";
    
    // Act
    await repository.UpdateAsync(originalCause);
    
    // Assert
    var updatedCause = await db.Causes.FindAsync(1);
    Assert.NotNull(updatedCause);
    Assert.Equal("Mental Health", updatedCause.Name);
  }
  
  [Fact]
  public async Task DeleteCauseAsync_DeletesExistingCause()
  {
    // Arrange
    var db = GetDbContext();
    var cause = new Cause
    {
      Id = 1,
      Name = "Adult Mental Health",
      Description = "Cause for adults suffering from mental health issues.",
      ImageUrl = "causeImage.jpg"
    };
    db.Causes.Add(cause);
    await db.SaveChangesAsync();
    
    var repository = new CauseRepository(db);
    
    // Act
    await repository.DeleteAsync(1);
    
    // Assert
    var deletedCause = await db.Causes.FindAsync(1);
    Assert.Null(deletedCause);
  }
}