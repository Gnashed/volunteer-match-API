using volunteerMatch.Models;
using System.Collections.Generic;

namespace volunteerMatch.Data
{
    public static class CauseData
    {
        public static List<Cause> Causes = new List<Cause>
        {
            new Cause
            {
                Id = 1,
                Name = "Animals",
                ImageUrl = "https://www.mccanndogs.com/cdn/shop/articles/living-with-cats-dogs-689902.jpg?v=1680325849",
                Description = "vitae imperdiet augue vulputate vel. Morbi.",
                Organizations = new List<Organization>() // Empty list for now
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
        };
    }
}
