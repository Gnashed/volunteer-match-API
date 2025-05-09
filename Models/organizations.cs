using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace volunteerMatch.Models
{
    public class Organization
    {
        public int Id { get; set; }
        public int VolunteerId { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public bool IsFollowing { get; set; }

        public Volunteer Volunteer { get; set; }
        public ICollection<OrganizationCause> OrganizationCauses { get; set; }
        public ICollection<OrganizationFollower> OrganizationFollowers { get; set; }
  }
}
