using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace VolunteerMatch.Models;

// Models/Organization.cs
public class Organization
{
    public int Id { get; set; }
    public int VolunteerId { get; set; }
    public string Name { get; set; }
    public string ImageURL { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public int CauseId { get; set; }
    public bool IsFollowing { get; set; }

    public Volunteer Volunteer { get; set; }
    public Cause Cause { get; set; }
    public ICollection<OrganizationFollower> OrganizationFollowers { get; set; }
}
