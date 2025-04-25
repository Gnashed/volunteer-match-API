// Data/VolunteerMatchDbContext.cs

using Microsoft.EntityFrameworkCore;
using VolunteerMatch.Models;

namespace VolunteerMatch.Data;

public class VolunteerMatchDbContext : DbContext
{
    public VolunteerMatchDbContext(DbContextOptions<VolunteerMatchDbContext> options) : base(options) { }

    public DbSet<Volunteer> Volunteers { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Cause> Causes { get; set; }
    public DbSet<OrganizationFollower> OrganizationFollowers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Composite key for join table
        modelBuilder.Entity<OrganizationFollower>()
            .HasKey(of => new { of.VolunteerId, of.OrganizationId });

        // Relationships
        modelBuilder.Entity<OrganizationFollower>()
            .HasOne(of => of.Volunteer)
            .WithMany(v => v.OrganizationFollowers)
            .HasForeignKey(of => of.VolunteerId);

        modelBuilder.Entity<OrganizationFollower>()
            .HasOne(of => of.Organization)
            .WithMany(o => o.OrganizationFollowers)
            .HasForeignKey(of => of.OrganizationId);

        modelBuilder.Entity<Organization>()
            .HasOne(o => o.Volunteer)
            .WithMany() // You can replace this with a collection if needed
            .HasForeignKey(o => o.VolunteerId);

        modelBuilder.Entity<Organization>()
            .HasOne(o => o.Cause)
            .WithMany(c => c.Organizations)
            .HasForeignKey(o => o.CauseId);

        // Seed data
        modelBuilder.Entity<Cause>().HasData(
            new Cause
            {
                Id = 1,
                Name = "Animals",
                ImageUrl = "https://www.mccanndogs.com/cdn/shop/articles/living-with-cats-dogs-689902.jpg?v=1680325849",
                Description = "vitae imperdiet augue vulputate vel. Morbi.",
                Organizations = new List<Organization>()
            },
            new Cause
            {
                Id = 2,
                Name = "Children & Youth",
                ImageUrl = "https://cff2.earth.com/uploads/2018/07/25115124/Kids-now-spend-twice-as-much-time-playing-indoors-than-outdoors.jpg",
                Description = "vel dignissim nulla scelerisque a. Ut in ante mattis",
                Organizations = new List<Organization>()
            },
            new Cause
            {
                Id = 3,
                Name = "Cultural & Education",
                ImageUrl = "https://theculturalcourier.home.blog/wp-content/uploads/2019/07/culture-and-edcucation.png?w=640",
                Description = "consectetur adipiscing elit. Praesent rutrum nibh lacus",
                Organizations = new List<Organization>()
            },
            new Cause
            {
                Id = 4,
                Name = "Disabilities",
                ImageUrl = "https://www.stronggo.com/sites/default/files/2023-01/shutterstock_716571097.jpg",
                Description = "euismod mi. Phasellus blandit lacinia pharetra. Vestibulum mollis finibus metus",
                Organizations = new List<Organization>()
            },
            new Cause
            {
                Id = 5,
                Name = "Disaster Relief",
                ImageUrl = "https://imageio.forbes.com/blogs-images/dennismersereau/files/2017/03/C8GOcZ7VYAEAFbc-1200x900.jpg?format=jpg&height=600&width=1200&fit=bounds",
                Description = "consectetur adipiscing elit. Praesent rutrum nibh lacus",
                Organizations = new List<Organization>()
            },
            new Cause
            {
                Id = 6,
                Name = "Elderly",
                ImageUrl = "https://www.investopedia.com/thmb/V_F1Tyf6FqF6fV55_2P353jdjdA=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/GettyImages-557921071-5689c4873df78ccc15334caf.jpg",
                Description = "vitae imperdiet augue vulputate vel. Morbi.",
                Organizations = new List<Organization>()
            },
            new Cause
            {
                Id = 7,
                Name = "Environment",
                ImageUrl = "https://images.nationalgeographic.org/image/upload/v1638890315/EducationHub/photos/amazon-river-basin.jpg",
                Description = "Lorem ipsum dolor sit amet",
                Organizations = new List<Organization>()
            },
            new Cause
            {
                Id = 8,
                Name = "Homelessness",
                ImageUrl = "https://i0.wp.com/calmatters.org/wp-content/uploads/2023/07/071223-Homeless-Camp-LA-JAH-CM-40.jpg?fit=2000%2C1333&ssl=1",
                Description = "bibendum libero non",
                Organizations = new List<Organization>()
            },
            new Cause
            {
                Id = 9,
                Name = "Hunger & Poverty",
                ImageUrl = "https://i.unu.edu/media/ourworld.unu.edu-en/article/148/Hunger-and-poverty-overshadowed.jpg",
                Description = "bibendum libero non",
                Organizations = new List<Organization>()
            },
            new Cause
            {
                Id = 10,
                Name = "Health & Disease",
                ImageUrl = "https://scx2.b-cdn.net/gfx/news/hires/2018/3-bacteria.jpg",
                Description = "bibendum libero non",
                Organizations = new List<Organization>()
            }
        );

        modelBuilder.Entity<Volunteer>().HasData(
            new Volunteer
            {
                Id = 1,
                Uid = "8a46acbf-bea8-49ea-87e7-bd62d6352be5",
                FirstName = "Moshe",
                LastName = "Scotchforth",
                Email = "mscotchforth0@unicef.org",
                ImageUrl = "https://www.wsaz.com/resizer/TtxT5eHIfsdlCYPQRJG_wdDg9yQ=/arc-photo-gray/arc3-prod/public/FLBGRRRDQNHYBNTNHU4WOWRIFY.png",
                OrganizationFollowers = new List<OrganizationFollower>()
            },
            new Volunteer
            {
                Id = 2,
                Uid = "eabedf0d-6e38-4d3a-bd2b-c707ab8f79c0",
                FirstName = "Natalya",
                LastName = "Normanvell",
                Email = "nnormanvell1@un.org",
                ImageUrl = "https://www.wsaz.com/resizer/TtxT5eHIfsdlCYPQRJG_wdDg9yQ=/arc-photo-gray/arc3-prod/public/FLBGRRRDQNHYBNTNHU4WOWRIFY.png",
                OrganizationFollowers = new List<OrganizationFollower>()
            },
            new Volunteer
            {
                Id = 3,
                Uid = "2844319a-349d-4620-a18b-9976d751a0cb",
                FirstName = "Alyda",
                LastName = "Low",
                Email = "alow2@vkontakte.ru",
                ImageUrl = "https://www.wsaz.com/resizer/TtxT5eHIfsdlCYPQRJG_wdDg9yQ=/arc-photo-gray/arc3-prod/public/FLBGRRRDQNHYBNTNHU4WOWRIFY.png",
                OrganizationFollowers = new List<OrganizationFollower>()
            },
            new Volunteer
            {
                Id = 4,
                Uid = "5777d6e9-5c33-4c02-bf4d-65c688d09090",
                FirstName = "Roch",
                LastName = "Stavers",
                Email = "rstavers3@illinois.edu",
                ImageUrl = "https://www.wsaz.com/resizer/TtxT5eHIfsdlCYPQRJG_wdDg9yQ=/arc-photo-gray/arc3-prod/public/FLBGRRRDQNHYBNTNHU4WOWRIFY.png",
                OrganizationFollowers = new List<OrganizationFollower>()
            },
            new Volunteer
            {
                Id = 5,
                Uid = "465386f6-970b-46e3-a45a-c02c91d86fdc",
                FirstName = "Garrard",
                LastName = "Howman",
                Email = "ghowman4@ed.gov",
                ImageUrl = "https://www.wsaz.com/resizer/TtxT5eHIfsdlCYPQRJG_wdDg9yQ=/arc-photo-gray/arc3-prod/public/FLBGRRRDQNHYBNTNHU4WOWRIFY.png",
                OrganizationFollowers = new List<OrganizationFollower>()
            },
            new Volunteer
            {
                Id = 6,
                Uid = "b3119942-065c-4a74-bbb8-7a85f7eebd23",
                FirstName = "Alana",
                LastName = "Darling",
                Email = "adarling5@gnu.org",
                ImageUrl = "https://www.wsaz.com/resizer/TtxT5eHIfsdlCYPQRJG_wdDg9yQ=/arc-photo-gray/arc3-prod/public/FLBGRRRDQNHYBNTNHU4WOWRIFY.png",
                OrganizationFollowers = new List<OrganizationFollower>()
            },
            new Volunteer
            {
                Id = 7,
                Uid = "03aa171b-e27b-4f65-8abb-6f9070213ad5",
                FirstName = "Malachi",
                LastName = "Gothrup",
                Email = "mgothrup6@51.la",
                ImageUrl = "https://www.wsaz.com/resizer/TtxT5eHIfsdlCYPQRJG_wdDg9yQ=/arc-photo-gray/arc3-prod/public/FLBGRRRDQNHYBNTNHU4WOWRIFY.png",
                OrganizationFollowers = new List<OrganizationFollower>()
            },
            new Volunteer
            {
                Id = 8,
                Uid = "7bae8314-31c4-4e72-bb8b-c012f4ae808e",
                FirstName = "Bryon",
                LastName = "Shelmerdine",
                Email = "bshelmerdine7@woothemes.com",
                ImageUrl = "https://www.wsaz.com/resizer/TtxT5eHIfsdlCYPQRJG_wdDg9yQ=/arc-photo-gray/arc3-prod/public/FLBGRRRDQNHYBNTNHU4WOWRIFY.png",
                OrganizationFollowers = new List<OrganizationFollower>()
            },
            new Volunteer
            {
                Id = 9,
                Uid = "75173053-1802-4ab3-957a-7bbe64992b64",
                FirstName = "Abigail",
                LastName = "D'Alesio",
                Email = "adalesio8@smh.com.au",
                ImageUrl = "https://www.wsaz.com/resizer/TtxT5eHIfsdlCYPQRJG_wdDg9yQ=/arc-photo-gray/arc3-prod/public/FLBGRRRDQNHYBNTNHU4WOWRIFY.png",
                OrganizationFollowers = new List<OrganizationFollower>()
            },
            new Volunteer
            {
                Id = 10,
                Uid = "0175f3a4-ed7f-488e-8f1c-65c47106e994",
                FirstName = "Valeda",
                LastName = "Disley",
                Email = "vdisley9@skype.com",
                ImageUrl = "https://www.wsaz.com/resizer/TtxT5eHIfsdlCYPQRJG_wdDg9yQ=/arc-photo-gray/arc3-prod/public/FLBGRRRDQNHYBNTNHU4WOWRIFY.png",
                OrganizationFollowers = new List<OrganizationFollower>()
            }
        );

        modelBuilder.Entity<Organization>().HasData(
            new Organization
            {
                Id = 1,
                VolunteerId = 0, // Empty for now
                Name = "Feeding America",
                ImageURL = "https://eu-images.contentstack.com/v3/assets/blte5a51c2d28bbcc9c/bltbf9b120b9c69987f/6647853f1bf9d6ed16038bc6/Feeding_America.png?disable=upscale&width=1200&height=630&fit=crop",
                Description = "Duis bibendum. Morbi non quam nec dui luctus rutrum. Nulla tellus.",
                Location = "Room 1448",
                CauseId = 1, // CauseId can be mapped later
                IsFollowing = true,
                OrganizationFollowers = new List<OrganizationFollower>()
            },
            new Organization
            {
                Id = 2,
                VolunteerId = 0,
                Name = "Good 360",
                ImageURL = "https://good360.org/wp-content/uploads/2020/11/Good360-Logo-500x.jpg",
                Description = "Phasellus sit amet erat. Nulla tempus. Vivamus in felis eu sapien cursus vestibulum.",
                Location = "5th Floor",
                CauseId = 2,
                IsFollowing = true,
                OrganizationFollowers = new List<OrganizationFollower>()
            },
            new Organization
            {
                Id = 3,
                VolunteerId = 0,
                Name = "St. Jude Children's Research Hospital",
                ImageURL = "https://mma.prnewswire.com/media/613525/St_Jude_Childrens_Research_Hospital_Logo.jpg?p=twitter",
                Description = "Fusce posuere felis sed lacus. Morbi sem mauris, laoreet ut, rhoncus aliquet, pulvinar sed, nisl. Nunc rhoncus dui vel sem.",
                Location = "Suite 29",
                CauseId = 3,
                OrganizationFollowers = new List<OrganizationFollower>()
            },
            new Organization
            {
                Id = 4,
                VolunteerId = 0,
                Name = "United Way Worldwide",
                ImageURL = "https://upload.wikimedia.org/wikipedia/commons/thumb/c/cd/United_Way_Worldwide_logo.svg/1200px-United_Way_Worldwide_logo.svg.png",
                Description = "Fusce posuere felis sed lacus. Morbi sem mauris, laoreet ut, rhoncus aliquet, pulvinar sed, nisl. Nunc rhoncus dui vel sem.",
                Location = "Apt 1957",
                CauseId = 4,
                OrganizationFollowers = new List<OrganizationFollower>()
            },
            new Organization
            {
                Id = 5,
                VolunteerId = 0,
                Name = "Direct Relief",
                ImageURL = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS2fOL-c3xiQ-mY3AY8sZW2f-Wazzmz_tP6cw&s",
                Description = "Fusce consequat. Nulla nisl. Nunc nisl.",
                Location = "Room 399",
                CauseId = 5,
                OrganizationFollowers = new List<OrganizationFollower>()
            },
            new Organization
            {
                Id = 6,
                VolunteerId = 0,
                Name = "Salvation Army",
                ImageURL = "https://upload.wikimedia.org/wikipedia/en/thumb/c/c4/The_Salvation_Army.svg/1200px-The_Salvation_Army.svg.png",
                Description = "In congue. Etiam justo. Etiam pretium iaculis justo.",
                Location = "Room 418",
                CauseId = 6,
                OrganizationFollowers = new List<OrganizationFollower>()
            },
            new Organization
            {
                Id = 7,
                VolunteerId = 0,
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
                VolunteerId = 0,
                Name = "Americares",
                ImageURL = "https://www.americares.org/wp-content/uploads/Americares-Logo_02big.gif",
                Description = "Integer tincidunt ante vel ipsum. Praesent blandit lacinia erat. Vestibulum sed magna at nunc commodo placerat.",
                Location = "17th Floor",
                CauseId = 8,
                OrganizationFollowers = new List<OrganizationFollower>()
            },
            new Organization
            {
                Id = 9,
                VolunteerId = 0,
                Name = "Goodwill Industries International",
                ImageURL = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTv032LFny9_SAG8KQK6-jiOd4Ku65DmBX43Q&s",
                Description = "Fusce posuere felis sed lacus. Morbi sem mauris, laoreet ut, rhoncus aliquet, pulvinar sed, nisl. Nunc rhoncus dui vel sem.",
                Location = "Suite 68",
                CauseId = 9,
                OrganizationFollowers = new List<OrganizationFollower>()
            },
            new Organization
            {
                Id = 10,
                VolunteerId = 0,
                Name = "Boys & Girls Clubs of America",
                ImageURL = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQNPs6VOZGUeyc0jibA4r7JN-2zt_BP_OAssQ&s",
                Description = "In congue. Etiam justo. Etiam pretium iaculis justo.",
                Location = "Suite 37",
                CauseId = 10,
                OrganizationFollowers = new List<OrganizationFollower>()
            }
        );

        modelBuilder.Entity<OrganizationFollower>().HasData(
            new OrganizationFollower
            {
                VolunteerId = 2,
                OrganizationId = 1
            }
        );
    }
}