namespace VolunteerMatch.Models.Requests;

public class CreateOrganizationRequest
{
  public string Name { get; set; }
  public string Description { get; set; }
  public string ImageURL { get; set; }
  public string Location { get; set; }
  public bool IsFollowing { get; set; }
  public int CauseId { get; set; }
}

public class UpdateOrganizationRequest
{
  public string Name { get; set; }
  public string ImageURL { get; set; }
  public string Description { get; set; }
  public string Location { get; set; }
  public int CauseId { get; set; }
  public bool IsFollowing { get; set; }
}