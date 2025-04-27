using VolunteerMatch.Utils;
using Xunit;

namespace VolunteerMatch.VolunteerMatch.Tests;

public class CheckForDuplicateTests
{
    [Fact]
    public void Check_Duplicate_Name_ReturnTrue()
    {
        // Arrange - set up data/ condition needed for testing.
        var test = new CheckForDuplicate();
        
        // Act - Call the method
        var result = test.IsDuplicate("Feeding America");
        
        // Assert - Verify the result
        Assert.True(result);
    }
}
