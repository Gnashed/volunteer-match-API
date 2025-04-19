using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VolunteerMatch.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Causes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Causes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OrganizationFollowers",
                keyColumns: new[] { "OrganizationId", "VolunteerId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Volunteers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Causes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Volunteers",
                keyColumn: "Id",
                keyValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Causes",
                columns: new[] { "Id", "Description", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { 1, "Protecting the planet", "env.jpg", "Environment" },
                    { 2, "Supporting learning", "edu.jpg", "Education" },
                    { 3, "Helping pets and wildlife", "animals.jpg", "Animals" }
                });

            migrationBuilder.InsertData(
                table: "Volunteers",
                columns: new[] { "Id", "Email", "FirstName", "ImageUrl", "LastName", "Uid" },
                values: new object[,]
                {
                    { 1, "alex@example.com", "Alex", "alex.jpg", "Johnson", "abc123" },
                    { 2, "taylor@example.com", "Taylor", "taylor.jpg", "Smith", "def456" }
                });

            migrationBuilder.InsertData(
                table: "Organizations",
                columns: new[] { "Id", "CauseId", "Description", "ImageURL", "IsFollowing", "Location", "Name", "VolunteerId" },
                values: new object[] { 1, 3, "Rescue and rehome cats", "cats.jpg", false, "Nashville", "Save the Cats", 1 });

            migrationBuilder.InsertData(
                table: "OrganizationFollowers",
                columns: new[] { "OrganizationId", "VolunteerId" },
                values: new object[] { 1, 2 });
        }
    }
}
