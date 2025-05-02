using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace volunteerMatch.Models
{
    public class OrganizationSeedDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public string Location { get; set; }
        public bool IsFollowing { get; set; }
        public List<int> CauseIds { get; set; }
    }
}
