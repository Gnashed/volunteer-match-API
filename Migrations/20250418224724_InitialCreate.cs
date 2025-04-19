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
                    VolunteerId = table.Column<int>(type: "integer", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_Organizations_Volunteers_VolunteerId",
                        column: x => x.VolunteerId,
                        principalTable: "Volunteers",
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

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationFollowers_OrganizationId",
                table: "OrganizationFollowers",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_CauseId",
                table: "Organizations",
                column: "CauseId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_VolunteerId",
                table: "Organizations",
                column: "VolunteerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrganizationFollowers");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "Causes");

            migrationBuilder.DropTable(
                name: "Volunteers");
        }
    }
}
