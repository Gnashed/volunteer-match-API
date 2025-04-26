namespace VolunteerMatch.Models.Requests;

public class GetCauseRequests
{
  public int Id { get; set; }
  public string Name { get; set; }
  public string ImageUrl { get; set; }
  public string Description { get; set; }
}
public class CreateCauseRequest
{
  public string Name { get; set; }
  public string ImageUrl { get; set; }
  public string Description { get; set; }
}