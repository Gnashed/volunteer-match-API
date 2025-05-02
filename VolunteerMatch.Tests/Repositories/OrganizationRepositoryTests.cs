using Xunit;
using VolunteerMatch.Data.Repositories;
using VolunteerMatch.Data;
using VolunteerMatch.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace VolunteerMatch.Tests.Repositories;

public class OrganizationRepositoryTests
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
  public async Task GetAllAsync_ReturnsAllOrganizations()
  {
    // Arrange
    var db = GetDbContext();
    db.Organizations.AddRange(new List<Organization>
    {
      new Organization 
      { 
        Id = 1,
        Name = "Warzone Group",
        Description = "Lorem ipsum fepaeov ewfwun rmfw",
        ImageURL = "wzgrp.jpg",
        Location = "Orlando, FL",
        IsFollowing = false,
        CauseId = 7
      },
      new Organization
      { 
        Id = 2,
        Name = "Test Group 2",
        Description = "Lorem ipsum fepaeov ewfwun rmfw",
        ImageURL = "tg2.jpg",
        Location = "Orlando, FL",
        IsFollowing = false,
        CauseId = 2
      },
      new Organization
      { 
        Id = 3,
        Name = "Test Group 3",
        Description = "Lorem ipsum fepaeov ewfwun rmfw",
        ImageURL = "tg2.jpg",
        Location = "Detroit, MI",
        IsFollowing = false,
        CauseId = 10
      },
      new Organization
      { 
        Id = 4,
        Name = "Test Group 4",
        Description = "Lorem ipsum fepaeov ewfwun rmfw",
        ImageURL = "tg4.jpg",
        Location = "Chicago, IL",
        IsFollowing = false,
        CauseId = 1
      }
    });
    await db.SaveChangesAsync();
    var repository = new OrganizationRepository(db);
    
    // Act
    var organizations = await repository.GetAllAsync();
    
    // Assert
    Assert.Equal(4, organizations.Count);
  }

  [Fact]
  public async Task GetByIdAsync_ReturnsOrganization()
  {
    // Arrange
    var db = GetDbContext();
    db.Organizations.Add(new Organization
    {
      Id = 1,
      Name = "Change Group",
      Description = "Lorem ipsum fepaeov ewfwun rmfw",
      ImageURL = "chance_group.jpg",
      Location = "Miami, FL",
      IsFollowing = false,
      CauseId = 20,
    });
    await db.SaveChangesAsync();
    var repository = new OrganizationRepository(db);
    
    // Act
    var organization = await repository.GetByIdAsync(1);
    
    // Assert
    Assert.NotNull(organization);
    Assert.Equal("Change Group", organization.Name);
  }

  [Fact]
  public async Task GetByIdAsync_ReturnsNullWhenNotFound()
  {
    // Arrange
    var db = GetDbContext();
    var repository = new OrganizationRepository(db);

    // Act
    var organization = await repository.GetByIdAsync(999);
    
    // Assert
    Assert.Null(organization);
  }

  [Fact]
  public async Task AddAsync_AddsNewOrganization()
  {
    // Arrange
    var db = GetDbContext();
    var repository = new OrganizationRepository(db);
    var newOrganization = new Organization
    {
      Id = 1,
      Name = "Children STEM Group",
      Description = "Lorem ipsum fepaeov ewfwun rmfw",
      ImageURL = "csg.jpg",
      Location = "Nashville, TN",
      IsFollowing = false,
      CauseId = 5,
    };
    
    // Act
    await repository.AddAsync(newOrganization);
    await db.SaveChangesAsync();
    
    // Assert
    await repository.GetByIdAsync(1);
    Assert.NotNull(newOrganization);
    Assert.Equal("Children STEM Group", newOrganization.Name);
  }

  [Fact]
  public async Task UpdateAsync_UpdatesOrganization()
  {
    // Arrange
    var db = GetDbContext();
    var originalOrganization = new Organization
    {
      Id = 1,
      Name = "Change Group",
      Description = "Lorem ipsum fepaeov ewfwun rmfw",
      ImageURL = "changegroup.jpg",
      Location = "Nashville, TN",
      IsFollowing = false,
      CauseId = 5,
    };
    db.Organizations.Add(originalOrganization);
    await db.SaveChangesAsync();
    
    var repository = new OrganizationRepository(db);
    originalOrganization.Name = "Physical Health Group";
    
    // Act
    await repository.UpdateAsync(originalOrganization);
    await db.SaveChangesAsync();
    
    // Assert
    var updatedOrganization = await repository.GetByIdAsync(1);
    Assert.NotNull(updatedOrganization);
    Assert.Equal("Physical Health Group", updatedOrganization.Name);
  }

  [Fact]
  public async Task DeleteAsync_DeletesOrganization()
  {
    // Arrange
    var db = GetDbContext();
    var organization = new Organization
    {
      Id = 1,
      Name = "Change Group",
      Description = "Lorem ipsum fepaeov ewfwun rmfw",
      ImageURL = "changegroup.jpg",
      Location = "Nashville, TN",
      IsFollowing = false,
      CauseId = 5
    };
    db.Organizations.Add(organization);
    await db.SaveChangesAsync();
    
    var repository = new OrganizationRepository(db);
    
    // Act
    await repository.DeleteAsync(1);
    
    // Assert
    var deletedOrganization = await db.Organizations.FindAsync(1);
    Assert.Null(deletedOrganization);
  }
}
