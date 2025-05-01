/*
 *
 * 
 * When unit testing a controller, you can mock its dependency (ICauseRepository) using a mock.
 * 
 * Moq (a mocking library) will help with mocking the repository, and then we can verify that the controller methods 
   return the expected IActionResult types (Ok, NotFound, BadRequest, etc.).
 *
 * When mocking with repository this way, unit tests are fast, isolated, and not reliant on EF Core.
 * 
 */

using Xunit;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VolunteerMatch.Controllers;
using VolunteerMatch.Data.Repositories.Interfaces;
using VolunteerMatch.Models;
using VolunteerMatch.Models.DTOs;
using VolunteerMatch.Models.Requests;

namespace VolunteerMatch.Tests.Controllers;

public class CauseControllerTests
{
  [Fact]
  public async Task GetAllAsync_ReturnsOkResultWithListOfCauseDtos()
  {
    // ARRANGE
    
    // A mock object that implements ICauseRepository. This object will be used in the unit tests for simulating ... 
    // the behavior of a real repo without needing to access the db directly or other external resources.
    var mockRepo = new Mock<ICauseRepository>();
    /*
     * 
     * Setup - a method that can be overridden to setup the behavior of the mock. It tells the mock what to do when a ...
       specific method is called (GetAllAsync in this case).
       
     * repo (param) - Represents the mock instance of ICauseRepository, and repo.GetAllAsync is the method being targeted.
     *
     * .ReturnsAsync() -- Specifies what the mock should return when GetAllAsync() method is called.
     * 
    */ 
    mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Cause>
    {
      new Cause { Id = 1, Name = "Education", Description = "Desc", ImageUrl = "img1.jpg" },
      new Cause { Id = 2, Name = "Health", Description = "Desc", ImageUrl = "img2.jpg" }
    });
    // Create instance of a new controller. MockRepo.Object is passed to the constructor. It will retrieve the actual ...
    // mock object of type ICauseRepository that was set up earlier.
    var controller = new CauseController(mockRepo.Object);
    
    // ACT
    
    var result = await controller.GetAllAsyncTask();
    // Asserts that the `result` is of type OkObjectResult, which represents HTTP status code 200 in ASP.NET Core. If ...
    // successful, `okResult` will hold the casted value of result as OkObjectResult.
    var okResult = Assert.IsType<OkObjectResult>(result);
    // Asserts that the `Value` property is of type List<CauseDtos>. If successful, returnValue will hold the casted ...
    // value of okResult.Value as a list of CauseDto objects.
    var returnValue = Assert.IsType<List<CauseDto>>(okResult.Value);
    
    // ASSERT
    Assert.Equal(2, returnValue.Count);
  }

  [Fact]
  public async Task GetByIdAsync_ReturnNotFoundWhenCauseDoesNotExist()
  {
    // Arrange
    var mockRepo = new Mock<ICauseRepository>();
    // It.IsAny<int>() --- A Moq matcher that allows the method to be called with any integer value.
    // (Cause?)null) --- indicates the method will return a Task that resolves to null, but explicitly casted to a
    // nullable Cause type.
    mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
      .ReturnsAsync((Cause?)null);
    
    var controller = new CauseController(mockRepo.Object);
    
    // Act
    var result = await controller.GetByIdTask(999);

    // Assert
    Assert.IsType<NotFoundResult>(result);
  }

  [Fact]
  public async Task AddAsync_ReturnsCreatedResultWhenCauseIsValid()
  {
    // Arrange
    var mockRepo = new Mock<ICauseRepository>();
    mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Cause>()))
      .Returns(Task.CompletedTask);
    
    var controller = new CauseController(mockRepo.Object);
    var request = new CreateCauseRequest
    {
      Name = "Education",
      Description = "Desc",
      ImageUrl = "img1.jpg"
    };
    
    // Act
    var result = await controller.PostAsyncTask(request);

    // Assert
    
    // CreatedAtActionResult --- Returns HTTP status code 201 action result.
    var createdResult = Assert.IsType<CreatedAtActionResult>(result);
    var dto = Assert.IsType<CauseDto>(createdResult.Value);
    Assert.Equal("Education",  dto.Name);
  }

  [Fact]
  public async Task UpdateAsync_ReturnsNotFoundWhenCauseDoesNotExist()
  {
    // Arrange
    var mockRepo = new Mock<ICauseRepository>();
    mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
      .ReturnsAsync((Cause?)null);
    
    var controller = new CauseController(mockRepo.Object);
    var request = new UpdateCauseRequest
    {
      Name = "Fitness",
      Description = "Desc",
      ImageUrl = "fitness.jpg"
    };
    
    // Act
    var result = await controller.PatchAsyncTask(1, request);

    // Assert
    Assert.IsType<NotFoundResult>(result);
  }

  [Fact]
  public async Task DeleteAsync_ReturnsNoContentWhenCauseExist()
  {
    // Arrange
    var mockRepo = new Mock<ICauseRepository>();
    mockRepo.Setup(repo => repo.GetByIdAsync(1))
      .ReturnsAsync(new Cause 
      { 
        Id = 1,
        Name = "Education",
        Description = "Desc",
        ImageUrl = "img1.jpg" 
      });
    mockRepo.Setup(repo => repo.DeleteAsync(1))
      .Returns(Task.CompletedTask);
    
    var controller = new CauseController(mockRepo.Object);
    
    // Act
    var result = await controller.DeleteAsyncTask(1);

    // Assert
    Assert.IsType<NoContentResult>(result);
  }
}

