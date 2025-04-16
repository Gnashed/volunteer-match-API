using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace VolunteerMatch.Models;

// Models/OrganizationFollower.cs
public class OrganizationFollower
{
    public int VolunteerId { get; set; }
    public Volunteer Volunteer { get; set; }

    public int OrganizationId { get; set; }
    public Organization Organization { get; set; }
}
