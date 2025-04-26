namespace VolunteerMatch.Models.Requests;

public class CreateCauseRequest
{
  public string Name { get; set; }
  public string ImageUrl { get; set; }
  public string Description { get; set; }
}

public class UpdateCauseRequest
{
  public string Name { get; set; }
  public string ImageUrl { get; set; }
  public string Description { get; set; }
}