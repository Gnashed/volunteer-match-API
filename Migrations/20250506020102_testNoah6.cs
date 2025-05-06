using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolunteerMatch.Migrations
{
    /// <inheritdoc />
    public partial class testNoah6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerFollowers_Volunteers_FollowedId",
                table: "VolunteerFollowers");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerFollowers_Volunteers_FollowerId",
                table: "VolunteerFollowers");

            migrationBuilder.AlterColumn<string>(
                name: "FollowedId",
                table: "VolunteerFollowers",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "FollowerId",
                table: "VolunteerFollowers",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Volunteers_Uid",
                table: "Volunteers",
                column: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerFollowers_Volunteers_FollowedId",
                table: "VolunteerFollowers",
                column: "FollowedId",
                principalTable: "Volunteers",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerFollowers_Volunteers_FollowerId",
                table: "VolunteerFollowers",
                column: "FollowerId",
                principalTable: "Volunteers",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerFollowers_Volunteers_FollowedId",
                table: "VolunteerFollowers");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerFollowers_Volunteers_FollowerId",
                table: "VolunteerFollowers");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Volunteers_Uid",
                table: "Volunteers");

            migrationBuilder.AlterColumn<int>(
                name: "FollowedId",
                table: "VolunteerFollowers",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "FollowerId",
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

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerFollowers_Volunteers_FollowerId",
                table: "VolunteerFollowers",
                column: "FollowerId",
                principalTable: "Volunteers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
