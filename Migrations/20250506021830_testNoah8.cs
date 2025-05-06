using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolunteerMatch.Migrations
{
    /// <inheritdoc />
    public partial class testNoah8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerFollowers_Volunteers_FollowedId",
                table: "VolunteerFollowers");

            migrationBuilder.AlterColumn<int>(
                name: "FollowedId",
                table: "VolunteerFollowers",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerFollowers_Volunteers_FollowedId",
                table: "VolunteerFollowers",
                column: "FollowedId",
                principalTable: "Volunteers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerFollowers_Volunteers_FollowedId",
                table: "VolunteerFollowers");

            migrationBuilder.AlterColumn<string>(
                name: "FollowedId",
                table: "VolunteerFollowers",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerFollowers_Volunteers_FollowedId",
                table: "VolunteerFollowers",
                column: "FollowedId",
                principalTable: "Volunteers",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
