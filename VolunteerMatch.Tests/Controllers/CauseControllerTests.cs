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
  
  // [Fact]
  //
  // [Fact]
  //
  // [Fact]
  //
  // [Fact]
}

