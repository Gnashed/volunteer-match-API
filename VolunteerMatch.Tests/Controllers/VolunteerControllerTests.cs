using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using VolunteerMatch.Controllers;
using VolunteerMatch.Data.Repositories.Interfaces;
using VolunteerMatch.Models;
using VolunteerMatch.Models.DTOs;
using VolunteerMatch.Models.Requests;

namespace VolunteerMatch.Tests.Controllers;

public class VolunteerControllerTests
{
  [Fact]
  public async Task GetAllAsync_ReturnsOkWithListOfVolunteers()
  {
    // Arrange
    
    
    // Act
    
    
    // Assert
    
    
  }

  [Fact]
  public async Task GetByIdAsync_ReturnsNotFoundWhenVolunteerDoesNotExist()
  {
    // Arrange
    
    
    // Act
    
    
    // Assert


  }

  [Fact]
  public async Task GetByIdAsync_ReturnsOkWithVolunteer()
  {
    // Arrange
    
    
    // Act
    
    
    // Assert


  }

  [Fact]
  public async Task AddAsync_ReturnsCreatedResultWhenVolunteerIsValid()
  {
    {
      // Arrange
      
    
      // Act
      
    
      // Assert
      

    }
  }

  [Fact]
  public async Task UpdateAsync_ReturnsNotFoundWhenVolunteerDoesNotExist()
  {
    {
      // Arrange
    
    
      // Act
    
    
      // Assert


    }
  }

  [Fact]
  public async Task DeleteAsync_ReturnsNoContentWhenVolunteerExist()
  {
    {
      // Arrange
    
    
      // Act
    
    
      // Assert


    }
  }
}