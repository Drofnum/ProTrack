using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProTrack.Data.Migrations
{
    public partial class BetaModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BetaOpportunity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjectName = table.Column<string>(nullable: true),
                    ShortDescription = table.Column<string>(nullable: true),
                    LongDescription = table.Column<string>(nullable: true),
                    DriverUrl = table.Column<string>(nullable: true),
                    QuickStartGuideUrl = table.Column<string>(nullable: true),
                    UserGuideUrl = table.Column<string>(nullable: true),
                    FirmwareUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BetaOpportunity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BetaOptIn",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BetaOpportunityId = table.Column<int>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BetaOptIn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BetaOptIn_BetaOpportunity_BetaOpportunityId",
                        column: x => x.BetaOpportunityId,
                        principalTable: "BetaOpportunity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BetaOptIn_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BetaOptIn_BetaOpportunityId",
                table: "BetaOptIn",
                column: "BetaOpportunityId");

            migrationBuilder.CreateIndex(
                name: "IX_BetaOptIn_UserId",
                table: "BetaOptIn",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BetaOptIn");

            migrationBuilder.DropTable(
                name: "BetaOpportunity");
        }
    }
}
