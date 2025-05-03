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
    var mockRepo = new Mock<IVolunteerRepository>();
    mockRepo.Setup(repo => repo.GetAllAsync())
      .ReturnsAsync(new List<Volunteer>
      {
        new Volunteer
        {
          Id = 1,
          Uid = "YhJpl---385696",
          FirstName = "Billy",
          LastName = "Bob",
          Email = "billy_bob@email.com",
          ImageUrl = "picture.jpg"
        },
        new Volunteer
        {
          Id = 2,
          Uid = "YhJpl---343023",
          FirstName = "Martin",
          LastName = "Lawrence",
          Email = "martin_lawrence@email.com",
          ImageUrl = "picture.jpg"
        },
        new Volunteer
        {
          Id = 3,
          Uid = "YhJpl---302193",
          FirstName = "Lebron",
          LastName = "James",
          Email = "lebron_james@email.com",
          ImageUrl = "picture.jpg"
        },
      });
    var controller = new VolunteerController(mockRepo.Object);

    // Act
    var result = await controller.GetAllAsyncTask();
    var okResult = Assert.IsType<OkObjectResult>(result);
    var volunteerDtos = Assert.IsType<List<VolunteerDto>>(okResult.Value);
    
    // Assert
    Assert.Equal(3, volunteerDtos.Count);
  }

  [Fact]
  public async Task GetByIdAsync_ReturnsNotFoundWhenVolunteerDoesNotExist()
  {
    // Arrange
    var mockRepo = new Mock<IVolunteerRepository>();
    mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
      .ReturnsAsync((Volunteer?)null);
    
    var controller = new VolunteerController(mockRepo.Object);
    
    // Act
    var result = await controller.GetByIdAsyncTask(793); 
    
    // Assert
    Assert.IsType<NotFoundResult>(result);
  }

  [Fact]
  public async Task GetByIdAsync_ReturnsOkWithVolunteer()
  {
    // Arrange
    var mockRepo = new Mock<IVolunteerRepository>();
    mockRepo.Setup(repo => repo.GetByIdAsync(29))
      .ReturnsAsync(new Volunteer
      {
        Id = 29,
        Uid = "YhJpl---987123",
        FirstName = "Tion",
        LastName = "Blackmon",
        Email = "tion_blackmon@email.com",
        ImageUrl = "picture.jpg"
      });
    
    var controller = new VolunteerController(mockRepo.Object);
    
    // Act
    var result = await controller.GetByIdAsyncTask(29);
    var okResult = Assert.IsType<OkObjectResult>(result);
    
    // Assert
    Assert.IsType<VolunteerDto>(okResult.Value);
  }

  [Fact]
  public async Task AddAsync_ReturnsCreatedResultWhenVolunteerIsValid()
  {
    {
      // Arrange
      var mockRepo = new Mock<IVolunteerRepository>();
      mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Volunteer>()))
        .Returns(Task.CompletedTask);

      var controller = new VolunteerController(mockRepo.Object);

      // Act
      var result = await controller.PostAsyncTask(new CreateVolunteerRequest
      {
        FirstName = "Tion",
        LastName = "Blackmon",
        Email =  "tion_blackmon@email.com",
        ImageUrl = "picture.jpg",
        Uid = "YhJpl---987123"
      });
      
      // Assert
      var createdResult = Assert.IsType<CreatedAtActionResult>(result);
      var volunteerDto = Assert.IsType<VolunteerDto>(createdResult.Value);
      
      Assert.Equal("Tion", volunteerDto.FirstName);
      Assert.Equal("Blackmon", volunteerDto.LastName);
      Assert.Equal("tion_blackmon@email.com", volunteerDto.Email);
      Assert.Equal("YhJpl---987123", volunteerDto.Uid);
      Assert.Equal("picture.jpg", volunteerDto.ImageUrl);
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
  public async Task UpdateAsync_ReturnsOkWhenVolunteerDoesExist()
  {
    // Arrange
    
    
    // Act
    
    
    // Assert
    
  }

  [Fact]
  public async Task DeleteAsync_ReturnsNoContentWhenVolunteerExist()
  {
    {
      // Arrange
      var mockRepo = new Mock<IVolunteerRepository>();
      mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
        .ReturnsAsync(new Volunteer
        {
          Id = 1,
          Uid = "YhJpl---432999",
          FirstName = "Jack",
          LastName = "Frost",
          Email = "jack_frost@email.com",
          ImageUrl = "picture.jpg"
        });
      
      var controller = new VolunteerController(mockRepo.Object);
      
      // Act
      var result = await controller.DeleteAsyncTask(1);
      
      // Assert
      Assert.IsType<NoContentResult>(result);
    }
  }
}