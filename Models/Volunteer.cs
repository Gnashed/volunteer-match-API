using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace VolunteerMatch.Models;

// Models/Volunteer.cs
public class Volunteer
{
    public int Id { get; set; }
    public string Uid { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string ImageUrl { get; set; }

    public ICollection<OrganizationFollower> OrganizationFollowers { get; set; }
}
