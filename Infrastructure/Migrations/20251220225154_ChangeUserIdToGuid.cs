using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserIdToGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserInput",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    WeightLbs = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HeightInches = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WaistInches = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NeckInches = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HipInches = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    ActivityLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInput", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserOutput",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InputId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CalculatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    BMI = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    BMR = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    TDEE = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    BFP = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    LBM = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    WtHR = table.Column<decimal>(type: "decimal(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOutput", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserOutput_UserInput_InputId",
                        column: x => x.InputId,
                        principalTable: "UserInput",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserInput_CreatedAt",
                table: "UserInput",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_UserInput_UserId",
                table: "UserInput",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOutput_CalculatedAt",
                table: "UserOutput",
                column: "CalculatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_UserOutput_InputId",
                table: "UserOutput",
                column: "InputId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOutput_UserId",
                table: "UserOutput",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserOutput");

            migrationBuilder.DropTable(
                name: "UserInput");
        }
    }
}
