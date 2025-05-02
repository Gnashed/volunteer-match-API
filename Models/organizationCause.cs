using System.ComponentModel.DataAnnotations;

namespace volunteerMatch.Models
{
    public class OrganizationCause
    {
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        
        public int CauseId { get; set; }
        public Cause Cause { get; set; }
    }
}
