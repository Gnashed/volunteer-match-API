namespace VolunteerMatch.Models.DTOs;

public class OrganizationDto
{
  // Remember this is the shape of the API responses for this entity.
  public int Id { get; set; }
  public string Name { get; set; }
  public string ImageURL { get; set; }
  public string Description { get; set; }
  public string Location { get; set; }
  public bool IsFollowing { get; set; }
  public int CauseId { get; set; }
}