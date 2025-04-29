using VolunteerMatch.Models;

namespace VolunteerMatch.Data.SampleData
{
    public static class OrganizationData
    {
        public static List<Organization> GetOrganizations()
        {
            return new List<Organization>
            {
                new Organization
                {
                    Id = 1,
                    // VolunteerId = 0, // Empty for now
                    Name = "Feeding America",
                    ImageURL =
                        "https://eu-images.contentstack.com/v3/assets/blte5a51c2d28bbcc9c/bltbf9b120b9c69987f/6647853f1bf9d6ed16038bc6/Feeding_America.png?disable=upscale&width=1200&height=630&fit=crop",
                    Description = "Duis bibendum. Morbi non quam nec dui luctus rutrum. Nulla tellus.",
                    Location = "Room 1448",
                    CauseId = 1, // CauseId can be mapped later
                    OrganizationFollowers = new List<OrganizationFollower>()
                },
                new Organization
                {
                    Id = 2,
                    // VolunteerId = 0,
                    Name = "Good 360",
                    ImageURL = "https://good360.org/wp-content/uploads/2020/11/Good360-Logo-500x.jpg",
                    Description =
                        "Phasellus sit amet erat. Nulla tempus. Vivamus in felis eu sapien cursus vestibulum.",
                    Location = "5th Floor",
                    CauseId = 2,
                    OrganizationFollowers = new List<OrganizationFollower>()
                },
                new Organization
                {
                    Id = 3,
                    // VolunteerId = 0,
                    Name = "St. Jude Children's Research Hospital",
                    ImageURL =
                        "https://mma.prnewswire.com/media/613525/St_Jude_Childrens_Research_Hospital_Logo.jpg?p=twitter",
                    Description =
                        "Fusce posuere felis sed lacus. Morbi sem mauris, laoreet ut, rhoncus aliquet, pulvinar sed, nisl. Nunc rhoncus dui vel sem.",
                    Location = "Suite 29",
                    CauseId = 3,
                    OrganizationFollowers = new List<OrganizationFollower>()
                },
                new Organization
                {
                    Id = 4,
                    // VolunteerId = 0,
                    Name = "United Way Worldwide",
                    ImageURL =
                        "https://upload.wikimedia.org/wikipedia/commons/thumb/c/cd/United_Way_Worldwide_logo.svg/1200px-United_Way_Worldwide_logo.svg.png",
                    Description =
                        "Fusce posuere felis sed lacus. Morbi sem mauris, laoreet ut, rhoncus aliquet, pulvinar sed, nisl. Nunc rhoncus dui vel sem.",
                    Location = "Apt 1957",
                    CauseId = 4,
                    OrganizationFollowers = new List<OrganizationFollower>()
                },
                new Organization
                {
                    Id = 5,
                    // VolunteerId = 0,
                    Name = "Direct Relief",
                    ImageURL =
                        "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS2fOL-c3xiQ-mY3AY8sZW2f-Wazzmz_tP6cw&s",
                    Description = "Fusce consequat. Nulla nisl. Nunc nisl.",
                    Location = "Room 399",
                    CauseId = 5,
                    OrganizationFollowers = new List<OrganizationFollower>()
                },
                new Organization
                {
                    Id = 6,
                    // VolunteerId = 0,
                    Name = "Salvation Army",
                    ImageURL =
                        "https://upload.wikimedia.org/wikipedia/en/thumb/c/c4/The_Salvation_Army.svg/1200px-The_Salvation_Army.svg.png",
                    Description = "In congue. Etiam justo. Etiam pretium iaculis justo.",
                    Location = "Room 418",
                    CauseId = 6,
                    OrganizationFollowers = new List<OrganizationFollower>()
                },
                new Organization
                {
                    Id = 7,
                    // VolunteerId = 0,
                    Name = "Habitat for Humanity International",
                    ImageURL = "https://over50andoverseas.com/wp-content/uploads/2018/11/habitat-for-humanity-logo.jpg",
                    Description = "Nulla ut erat id mauris vulputate elementum. Nullam varius. Nulla facilisi.",
                    Location = "PO Box 9244",
                    CauseId = 7,
                    OrganizationFollowers = new List<OrganizationFollower>()
                },
                new Organization
                {
                    Id = 8,
                    // VolunteerId = 0,
                    Name = "Americares",
                    ImageURL = "https://www.americares.org/wp-content/uploads/Americares-Logo_02big.gif",
                    Description =
                        "Integer tincidunt ante vel ipsum. Praesent blandit lacinia erat. Vestibulum sed magna at nunc commodo placerat.",
                    Location = "17th Floor",
                    CauseId = 8,
                    OrganizationFollowers = new List<OrganizationFollower>()
                },
                new Organization
                {
                    Id = 9,
                    // VolunteerId = 0,
                    Name = "Goodwill Industries International",
                    ImageURL =
                        "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTv032LFny9_SAG8KQK6-jiOd4Ku65DmBX43Q&s",
                    Description =
                        "Fusce posuere felis sed lacus. Morbi sem mauris, laoreet ut, rhoncus aliquet, pulvinar sed, nisl. Nunc rhoncus dui vel sem.",
                    Location = "Suite 68",
                    CauseId = 9,
                    OrganizationFollowers = new List<OrganizationFollower>()
                },
                new Organization
                {
                    Id = 10,
                    // VolunteerId = 10,
                    Name = "Boys & Girls Clubs of America",
                    ImageURL =
                        "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQNPs6VOZGUeyc0jibA4r7JN-2zt_BP_OAssQ&s",
                    Description = "In congue. Etiam justo. Etiam pretium iaculis justo.",
                    Location = "Suite 37",
                    CauseId = 10,
                    OrganizationFollowers = new List<OrganizationFollower>()
                }
            };
        }
    }
}