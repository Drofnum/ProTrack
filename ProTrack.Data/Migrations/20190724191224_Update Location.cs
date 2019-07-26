using Microsoft.EntityFrameworkCore.Migrations;

namespace ProTrack.Data.Migrations
{
    public partial class UpdateLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Manufacturers_ManufacturerId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_AspNetUsers_ApplicationUserId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_ApplicationUserId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Devices_ManufacturerId",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "ManufacturerId",
                table: "Devices");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "ManufacturerId",
                table: "Devices",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_ApplicationUserId",
                table: "Locations",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_ManufacturerId",
                table: "Devices",
                column: "ManufacturerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Manufacturers_ManufacturerId",
                table: "Devices",
                column: "ManufacturerId",
                principalTable: "Manufacturers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_AspNetUsers_ApplicationUserId",
                table: "Locations",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
