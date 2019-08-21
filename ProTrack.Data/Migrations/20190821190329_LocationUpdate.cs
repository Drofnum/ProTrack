using Microsoft.EntityFrameworkCore.Migrations;

namespace ProTrack.Data.Migrations
{
    public partial class LocationUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ApplicationUser",
                table: "Locations",
                newName: "ApplicationUserId");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Locations",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_ApplicationUserId",
                table: "Locations",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_AspNetUsers_ApplicationUserId",
                table: "Locations",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_AspNetUsers_ApplicationUserId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_ApplicationUserId",
                table: "Locations");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Locations",
                newName: "ApplicationUser");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUser",
                table: "Locations",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
