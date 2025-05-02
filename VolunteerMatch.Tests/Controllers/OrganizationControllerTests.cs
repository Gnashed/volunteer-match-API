using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using VolunteerMatch.Controllers;
using VolunteerMatch.Data.Repositories.Interfaces;
using VolunteerMatch.Models;
using VolunteerMatch.Models.DTOs;
using VolunteerMatch.Models.Requests;

namespace VolunteerMatch.Tests.Controllers;

public class OrganizationControllerTests
{
  [Fact]
  public async Task GetAllAsync_ReturnsOkWithListOfOrganizations()
  {
    // Arrange
    var mockRepo = new Mock<IOrganizationRepository>();
    mockRepo.Setup(repo => repo.GetAllAsync())
      .ReturnsAsync(new List<Organization>
      {
        new Organization
        {
          Id = 1,
          CauseId = 2,
          Description = "Nashville Leadership Group is the leading organization in ...",
          ImageURL = "image1.jpg",
          Name = "Nashville Leadership Group",
          Location = "Nashville, TN",
          IsFollowing = false
        },
        new Organization {
          Id = 2,
          CauseId = 2,
          Description = "MYG Leadership Group is the leading organization in ...",
          ImageURL = "image2.jpg",
          Name = "Miami Youth Group",
          Location = "Miami, FL",
          IsFollowing = false
        },
        new Organization {
          Id = 3,
          CauseId = 8,
          Description = "Since 2003, Disabled Veterans group ...",
          ImageURL = "image3.jpg",
          Name = "Disabled Veterans Group",
          Location = "Detroit, MI",
          IsFollowing = false
        }
      });
    var controller = new OrganizationController(mockRepo.Object);
    
    // Act
    var result = await controller.GetAllAsync();
    var okResult = Assert.IsType<OkObjectResult>(result);
    var returnValue = Assert.IsType<List<OrganizationDto>>(okResult.Value);
    
    // Assert
    Assert.Equal(3, returnValue.Count);
  }

  // [Fact]
  // public async Task GetByIdAsync_ReturnsNotFoundWhenOrganizationDoesNotExist()
  // {
  //   // Arrange
  //   
  //   
  //   // Act
  //   
  //   
  //   // Assert
  //
  //
  // }
  //
  // [Fact]
  // public async Task GetByIdAsync_ReturnsOkWithOrganization()
  // {
  //   // Arrange
  //   
  //   
  //   // Act
  //   
  //   
  //   // Assert
  //
  //
  // }
  //
  // [Fact]
  // public async Task AddAsync_ReturnsCreatedResultWhenOrganizationIsValid()
  // {
  //   {
  //     // Arrange
  //     
  //   
  //     // Act
  //     
  //   
  //     // Assert
  //     
  //
  //   }
  // }
  //
  // [Fact]
  // public async Task UpdateAsync_ReturnsNotFoundWhenOrganizationDoesNotExist()
  // {
  //   {
  //     // Arrange
  //   
  //   
  //     // Act
  //   
  //   
  //     // Assert
  //
  //
  //   }
  // }
  //
  // [Fact]
  // public async Task DeleteAsync_ReturnsNoContentWhenOrganizationExist()
  // {
  //   {
  //     // Arrange
  //   
  //   
  //     // Act
  //   
  //   
  //     // Assert
  //
  //
  //   }
  // }
}
