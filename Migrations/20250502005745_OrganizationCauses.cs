using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolunteerMatch.Migrations
{
    /// <inheritdoc />
    public partial class OrganizationCauses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrganizationCauses",
                columns: table => new
                {
                    OrganizationId = table.Column<int>(type: "integer", nullable: false),
                    CauseId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationCauses", x => new { x.OrganizationId, x.CauseId });
                    table.ForeignKey(
                        name: "FK_OrganizationCauses_Causes_CauseId",
                        column: x => x.CauseId,
                        principalTable: "Causes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizationCauses_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VolunteerFollowers",
                columns: table => new
                {
                    FollowerId = table.Column<int>(type: "integer", nullable: false),
                    FollowedId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VolunteerFollowers", x => new { x.FollowerId, x.FollowedId });
                    table.ForeignKey(
                        name: "FK_VolunteerFollowers_Volunteers_FollowedId",
                        column: x => x.FollowedId,
                        principalTable: "Volunteers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VolunteerFollowers_Volunteers_FollowerId",
                        column: x => x.FollowerId,
                        principalTable: "Volunteers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationCauses_CauseId",
                table: "OrganizationCauses",
                column: "CauseId");

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerFollowers_FollowedId",
                table: "VolunteerFollowers",
                column: "FollowedId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrganizationCauses");

            migrationBuilder.DropTable(
                name: "VolunteerFollowers");
        }
    }
}
