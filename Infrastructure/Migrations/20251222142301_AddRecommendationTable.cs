using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRecommendationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserRecommendation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BmiRecommendation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BmrRecommendation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TdeeRecommendation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BfpRecommendation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LbmRecommendation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WtHrRecommendation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRecommendation", x => x.Id);
                });



            migrationBuilder.CreateIndex(
                name: "IX_UserRecommendation_UserId",
                table: "UserRecommendation",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRecommendation");
        }
    }
}
