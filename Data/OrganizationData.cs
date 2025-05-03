using volunteerMatch.Models;
using System.Collections.Generic;

namespace volunteerMatch.Data
{
    public static class OrganizationData
    {
        public static List<OrganizationSeedDto> Organizations = new List<OrganizationSeedDto>
        {
            new OrganizationSeedDto
            {
                Id          = 1,
                Name        = "Feeding America",
                Description = "Duis bibendum. Morbi non quam nec dui luctus rutrum. Nulla tellus.",
                ImageURL    = "https://eu-images.contentstack.com/v3/assets/blte5a51c2d28bbcc9c/bltbf9b120b9c69987f/6647853f1bf9d6ed16038bc6/Feeding_America.png?disable=upscale&width=1200&height=630&fit=crop",
                Location    = "Room 1448",
                IsFollowing = true,
                CauseIds    = new List<int> { 1 }
            },
            new OrganizationSeedDto
            {
                Id          = 2,
                Name        = "Good 360",
                Description = "Phasellus sit amet erat. Nulla tempus. Vivamus in felis eu sapien cursus vestibulum.",
                ImageURL    = "https://good360.org/wp-content/uploads/2020/11/Good360-Logo-500x.jpg",
                Location    = "5th Floor",
                IsFollowing = true,
                CauseIds    = new List<int> { 2 }
            },
            new OrganizationSeedDto
            {
                Id          = 3,
                Name        = "St. Jude Children's Research Hospital",
                Description = "Fusce posuere felis sed lacus. Morbi sem mauris, laoreet ut, rhoncus aliquet, pulvinar sed, nisl. Nunc rhoncus dui vel sem.",
                ImageURL    = "https://mma.prnewswire.com/media/613525/St_Jude_Childrens_Research_Hospital_Logo.jpg?p=twitter",
                Location    = "Suite 29",
                IsFollowing = true,
                CauseIds    = new List<int> { 3 }
            },
            new OrganizationSeedDto
            {
                Id          = 4,
                Name        = "United Way Worldwide",
                Description = "Fusce posuere felis sed lacus. Morbi sem mauris, laoreet ut, rhoncus aliquet, pulvinar sed, nisl. Nunc rhoncus dui vel sem.",
                ImageURL    = "https://upload.wikimedia.org/wikipedia/commons/thumb/c/cd/United_Way_Worldwide_logo.svg/1200px-United_Way_Worldwide_logo.svg.png",
                Location    = "Apt 1957",
                IsFollowing = true,
                CauseIds    = new List<int> { 4 }
            },
            new OrganizationSeedDto
            {
                Id          = 5,
                Name        = "Direct Relief",
                Description = "Fusce consequat. Nulla nisl. Nunc nisl.",
                ImageURL    = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS2fOL-c3xiQ-mY3AY8sZW2f-Wazzmz_tP6cw&s",
                Location    = "Room 399",
                IsFollowing = false,
                CauseIds    = new List<int> { 5 }
            },
            new OrganizationSeedDto
            {
                Id          = 6,
                Name        = "Salvation Army",
                Description = "In congue. Etiam justo. Etiam pretium iaculis justo.",
                ImageURL    = "https://upload.wikimedia.org/wikipedia/en/thumb/c/c4/The_Salvation_Army.svg/1200px-The_Salvation_Army.svg.png",
                Location    = "Room 418",
                IsFollowing = true,
                CauseIds    = new List<int> { 6 }
            },
            new OrganizationSeedDto
            {
                Id          = 7,
                Name        = "Habitat for Humanity International",
                Description = "Nulla ut erat id mauris vulputate elementum. Nullam varius. Nulla facilisi.",
                ImageURL    = "https://over50andoverseas.com/wp-content/uploads/2018/11/habitat-for-humanity-logo.jpg",
                Location    = "PO Box 9244",
                IsFollowing = true,
                CauseIds    = new List<int> { 7 }
            },
            new OrganizationSeedDto
            {
                Id          = 8,
                Name        = "Americares",
                Description = "Integer tincidunt ante vel ipsum. Praesent blandit lacinia erat. Vestibulum sed magna at nunc commodo placerat.",
                ImageURL    = "https://www.americares.org/wp-content/uploads/Americares-Logo_02big.gif",
                Location    = "17th Floor",
                IsFollowing = false,
                CauseIds    = new List<int> { 8 }
            },
            new OrganizationSeedDto
            {
                Id          = 9,
                Name        = "Goodwill Industries International",
                Description = "Fusce posuere felis sed lacus. Morbi sem mauris, laoreet ut, rhoncus aliquet, pulvinar sed, nisl. Nunc rhoncus dui vel sem.",
                ImageURL    = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTv032LFny9_SAG8KQK6-jiOd4Ku65DmBX43Q&s",
                Location    = "Suite 68",
                IsFollowing = false,
                CauseIds    = new List<int> { 9 }
            },
            new OrganizationSeedDto
            {
                Id          = 10,
                Name        = "Boys & Girls Clubs of America",
                Description = "In congue. Etiam justo. Etiam pretium iaculis justo.",
                ImageURL    = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQNPs6VOZGUeyc0jibA4r7JN-2zt_BP_OAssQ&s",
                Location    = "Suite 37",
                IsFollowing = false,
                CauseIds    = new List<int> { 10 }
            }
        };
    }
}
