namespace VolunteerMatch.Models.Requests;

public class CreateVolunteerRequest
{
  public string Uid { get; set; }
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public string Email { get; set; }
  public string ImageUrl { get; set; }
}

public class UpdateVolunteerRequest
{
  public string Uid { get; set; }
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public string Email { get; set; }
  public string ImageUrl { get; set; }
}