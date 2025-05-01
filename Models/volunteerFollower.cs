using System.ComponentModel.DataAnnotations;

namespace volunteerMatch.Models
{
    public class VolunteerFollower
    {
        public int FollowerId { get; set; }
        public Volunteer Follower { get; set; }

        public int FollowedId { get; set; }
        public Volunteer Followed { get; set; }
    }
}
