using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VolunteerMatch.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Causes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Causes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Volunteers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Uid = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Volunteers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ImageURL = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: false),
                    CauseId = table.Column<int>(type: "integer", nullable: false),
                    IsFollowing = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organizations_Causes_CauseId",
                        column: x => x.CauseId,
                        principalTable: "Causes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationFollowers",
                columns: table => new
                {
                    VolunteerId = table.Column<int>(type: "integer", nullable: false),
                    OrganizationId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationFollowers", x => new { x.VolunteerId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_OrganizationFollowers_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizationFollowers_Volunteers_VolunteerId",
                        column: x => x.VolunteerId,
                        principalTable: "Volunteers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Causes",
                columns: new[] { "Id", "Description", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { 1, "vitae imperdiet augue vulputate vel. Morbi.", "https://www.mccanndogs.com/cdn/shop/articles/living-with-cats-dogs-689902.jpg?v=1680325849", "Animals" },
                    { 2, "vel dignissim nulla scelerisque a. Ut in ante mattis", "https://cff2.earth.com/uploads/2018/07/25115124/Kids-now-spend-twice-as-much-time-playing-indoors-than-outdoors.jpg", "Children & Youth" },
                    { 3, "consectetur adipiscing elit. Praesent rutrum nibh lacus", "https://theculturalcourier.home.blog/wp-content/uploads/2019/07/culture-and-edcucation.png?w=640", "Cultural & Education" },
                    { 4, "euismod mi. Phasellus blandit lacinia pharetra. Vestibulum mollis finibus metus", "https://www.stronggo.com/sites/default/files/2023-01/shutterstock_716571097.jpg", "Disabilities" },
                    { 5, "consectetur adipiscing elit. Praesent rutrum nibh lacus", "https://imageio.forbes.com/blogs-images/dennismersereau/files/2017/03/C8GOcZ7VYAEAFbc-1200x900.jpg?format=jpg&height=600&width=1200&fit=bounds", "Disaster Relief" },
                    { 6, "vitae imperdiet augue vulputate vel. Morbi.", "https://www.investopedia.com/thmb/V_F1Tyf6FqF6fV55_2P353jdjdA=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/GettyImages-557921071-5689c4873df78ccc15334caf.jpg", "Elderly" },
                    { 7, "Lorem ipsum dolor sit amet", "https://images.nationalgeographic.org/image/upload/v1638890315/EducationHub/photos/amazon-river-basin.jpg", "Environment" },
                    { 8, "bibendum libero non", "https://i0.wp.com/calmatters.org/wp-content/uploads/2023/07/071223-Homeless-Camp-LA-JAH-CM-40.jpg?fit=2000%2C1333&ssl=1", "Homelessness" },
                    { 9, "bibendum libero non", "https://i.unu.edu/media/ourworld.unu.edu-en/article/148/Hunger-and-poverty-overshadowed.jpg", "Hunger & Poverty" },
                    { 10, "bibendum libero non", "https://scx2.b-cdn.net/gfx/news/hires/2018/3-bacteria.jpg", "Health & Disease" }
                });

            migrationBuilder.InsertData(
                table: "Volunteers",
                columns: new[] { "Id", "Email", "FirstName", "ImageUrl", "LastName", "Uid" },
                values: new object[,]
                {
                    { 1, "mscotchforth0@unicef.org", "Moshe", "https://www.wsaz.com/resizer/TtxT5eHIfsdlCYPQRJG_wdDg9yQ=/arc-photo-gray/arc3-prod/public/FLBGRRRDQNHYBNTNHU4WOWRIFY.png", "Scotchforth", "8a46acbf-bea8-49ea-87e7-bd62d6352be5" },
                    { 2, "nnormanvell1@un.org", "Natalya", "https://www.wsaz.com/resizer/TtxT5eHIfsdlCYPQRJG_wdDg9yQ=/arc-photo-gray/arc3-prod/public/FLBGRRRDQNHYBNTNHU4WOWRIFY.png", "Normanvell", "eabedf0d-6e38-4d3a-bd2b-c707ab8f79c0" },
                    { 3, "alow2@vkontakte.ru", "Alyda", "https://www.wsaz.com/resizer/TtxT5eHIfsdlCYPQRJG_wdDg9yQ=/arc-photo-gray/arc3-prod/public/FLBGRRRDQNHYBNTNHU4WOWRIFY.png", "Low", "2844319a-349d-4620-a18b-9976d751a0cb" },
                    { 4, "rstavers3@illinois.edu", "Roch", "https://www.wsaz.com/resizer/TtxT5eHIfsdlCYPQRJG_wdDg9yQ=/arc-photo-gray/arc3-prod/public/FLBGRRRDQNHYBNTNHU4WOWRIFY.png", "Stavers", "5777d6e9-5c33-4c02-bf4d-65c688d09090" },
                    { 5, "ghowman4@ed.gov", "Garrard", "https://www.wsaz.com/resizer/TtxT5eHIfsdlCYPQRJG_wdDg9yQ=/arc-photo-gray/arc3-prod/public/FLBGRRRDQNHYBNTNHU4WOWRIFY.png", "Howman", "465386f6-970b-46e3-a45a-c02c91d86fdc" },
                    { 6, "adarling5@gnu.org", "Alana", "https://www.wsaz.com/resizer/TtxT5eHIfsdlCYPQRJG_wdDg9yQ=/arc-photo-gray/arc3-prod/public/FLBGRRRDQNHYBNTNHU4WOWRIFY.png", "Darling", "b3119942-065c-4a74-bbb8-7a85f7eebd23" },
                    { 7, "mgothrup6@51.la", "Malachi", "https://www.wsaz.com/resizer/TtxT5eHIfsdlCYPQRJG_wdDg9yQ=/arc-photo-gray/arc3-prod/public/FLBGRRRDQNHYBNTNHU4WOWRIFY.png", "Gothrup", "03aa171b-e27b-4f65-8abb-6f9070213ad5" },
                    { 8, "bshelmerdine7@woothemes.com", "Bryon", "https://www.wsaz.com/resizer/TtxT5eHIfsdlCYPQRJG_wdDg9yQ=/arc-photo-gray/arc3-prod/public/FLBGRRRDQNHYBNTNHU4WOWRIFY.png", "Shelmerdine", "7bae8314-31c4-4e72-bb8b-c012f4ae808e" },
                    { 9, "adalesio8@smh.com.au", "Abigail", "https://www.wsaz.com/resizer/TtxT5eHIfsdlCYPQRJG_wdDg9yQ=/arc-photo-gray/arc3-prod/public/FLBGRRRDQNHYBNTNHU4WOWRIFY.png", "D'Alesio", "75173053-1802-4ab3-957a-7bbe64992b64" },
                    { 10, "vdisley9@skype.com", "Valeda", "https://www.wsaz.com/resizer/TtxT5eHIfsdlCYPQRJG_wdDg9yQ=/arc-photo-gray/arc3-prod/public/FLBGRRRDQNHYBNTNHU4WOWRIFY.png", "Disley", "0175f3a4-ed7f-488e-8f1c-65c47106e994" }
                });

            migrationBuilder.InsertData(
                table: "Organizations",
                columns: new[] { "Id", "CauseId", "Description", "ImageURL", "IsFollowing", "Location", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Duis bibendum. Morbi non quam nec dui luctus rutrum. Nulla tellus.", "https://eu-images.contentstack.com/v3/assets/blte5a51c2d28bbcc9c/bltbf9b120b9c69987f/6647853f1bf9d6ed16038bc6/Feeding_America.png?disable=upscale&width=1200&height=630&fit=crop", false, "Room 1448", "Feeding America" },
                    { 2, 2, "Phasellus sit amet erat. Nulla tempus. Vivamus in felis eu sapien cursus vestibulum.", "https://good360.org/wp-content/uploads/2020/11/Good360-Logo-500x.jpg", false, "5th Floor", "Good 360" },
                    { 3, 3, "Fusce posuere felis sed lacus. Morbi sem mauris, laoreet ut, rhoncus aliquet, pulvinar sed, nisl. Nunc rhoncus dui vel sem.", "https://mma.prnewswire.com/media/613525/St_Jude_Childrens_Research_Hospital_Logo.jpg?p=twitter", false, "Suite 29", "St. Jude Children's Research Hospital" },
                    { 4, 4, "Fusce posuere felis sed lacus. Morbi sem mauris, laoreet ut, rhoncus aliquet, pulvinar sed, nisl. Nunc rhoncus dui vel sem.", "https://upload.wikimedia.org/wikipedia/commons/thumb/c/cd/United_Way_Worldwide_logo.svg/1200px-United_Way_Worldwide_logo.svg.png", false, "Apt 1957", "United Way Worldwide" },
                    { 5, 5, "Fusce consequat. Nulla nisl. Nunc nisl.", "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS2fOL-c3xiQ-mY3AY8sZW2f-Wazzmz_tP6cw&s", false, "Room 399", "Direct Relief" },
                    { 6, 6, "In congue. Etiam justo. Etiam pretium iaculis justo.", "https://upload.wikimedia.org/wikipedia/en/thumb/c/c4/The_Salvation_Army.svg/1200px-The_Salvation_Army.svg.png", false, "Room 418", "Salvation Army" },
                    { 7, 7, "Nulla ut erat id mauris vulputate elementum. Nullam varius. Nulla facilisi.", "https://over50andoverseas.com/wp-content/uploads/2018/11/habitat-for-humanity-logo.jpg", false, "PO Box 9244", "Habitat for Humanity International" },
                    { 8, 8, "Integer tincidunt ante vel ipsum. Praesent blandit lacinia erat. Vestibulum sed magna at nunc commodo placerat.", "https://www.americares.org/wp-content/uploads/Americares-Logo_02big.gif", false, "17th Floor", "Americares" },
                    { 9, 9, "Fusce posuere felis sed lacus. Morbi sem mauris, laoreet ut, rhoncus aliquet, pulvinar sed, nisl. Nunc rhoncus dui vel sem.", "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTv032LFny9_SAG8KQK6-jiOd4Ku65DmBX43Q&s", false, "Suite 68", "Goodwill Industries International" },
                    { 10, 10, "In congue. Etiam justo. Etiam pretium iaculis justo.", "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQNPs6VOZGUeyc0jibA4r7JN-2zt_BP_OAssQ&s", false, "Suite 37", "Boys & Girls Clubs of America" }
                });

            migrationBuilder.InsertData(
                table: "OrganizationFollowers",
                columns: new[] { "OrganizationId", "VolunteerId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 3 },
                    { 1, 4 },
                    { 2, 5 },
                    { 2, 6 },
                    { 2, 7 },
                    { 2, 8 },
                    { 3, 9 },
                    { 3, 10 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationFollowers_OrganizationId",
                table: "OrganizationFollowers",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_CauseId",
                table: "Organizations",
                column: "CauseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrganizationFollowers");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "Volunteers");

            migrationBuilder.DropTable(
                name: "Causes");
        }
    }
}
