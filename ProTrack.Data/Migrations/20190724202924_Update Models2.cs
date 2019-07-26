using Microsoft.EntityFrameworkCore.Migrations;

namespace ProTrack.Data.Migrations
{
    public partial class UpdateModels2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Manufacturers_DeviceTypes_DeviceTypeId",
                table: "Manufacturers");

            migrationBuilder.DropIndex(
                name: "IX_Manufacturers_DeviceTypeId",
                table: "Manufacturers");

            migrationBuilder.DropColumn(
                name: "DeviceTypeId",
                table: "Manufacturers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeviceTypeId",
                table: "Manufacturers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Manufacturers_DeviceTypeId",
                table: "Manufacturers",
                column: "DeviceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Manufacturers_DeviceTypes_DeviceTypeId",
                table: "Manufacturers",
                column: "DeviceTypeId",
                principalTable: "DeviceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
